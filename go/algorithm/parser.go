package algorithm

import (
	"fmt"
	"strings"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/flow"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/mathbase"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/operator"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals/functions/value"
)

// astNode 语法树节点。
type astNode struct {
	kind           string
	start          int
	end            int
	text           string
	value          Decimal
	unit           string
	hasUnit        bool
	op             string
	keyword        string
	children       []*astNode
	keyIsParameter bool
	key            string
}

// toFunction 将语法树转换为可执行节点。
func (n *astNode) toFunction() Function {
	switch n.kind {
	case "num":
		if n.hasUnit {
			return value.NewFunctionNumber2(n.value, n.unit)
		}
		return value.NewFunctionNumber(CreateDecimal(n.value))
	case "string":
		return value.NewFunctionValueText(CreateString(n.text))
	case "param":
		return value.NewFunctionParameter(n.text)
	case "const2":
		switch n.keyword {
		case "TRUE":
			return value.NewFunctionValueBoolean(true)
		case "FALSE":
			return value.NewFunctionValueBoolean(false)
		case "ALGORITHMVERSION":
			return value.NewFunctionVersion()
		}
		return value.NewFunctionNull()
	case "const":
		switch n.keyword {
		case "E":
			return value.NewFunctionNumberE()
		case "PI":
			return value.NewFunctionNumberPI()
		case "RAND":
			return value.NewFunctionRAND()
		case "GUID":
			return value.NewFunctionGUID()
		case "NOW":
			return value.NewFunctionNOW()
		case "TODAY":
			return value.NewFunctionTODAY()
		}
		return value.NewFunctionNull()
	case "call":
		args := n.childFunctions()
		if n.keyword == "PARAMETER" {
			return value.NewFunctionDiyFunction(n.text, args)
		}
		if fn := internals.GetFunction(n.keyword, args); fn != nil {
			return fn
		}
		return value.NewFunctionDiyFunction(n.text, args)
	case "method":
		args := n.childFunctions()
		if n.keyword != "PARAMETER" {
			if fn := internals.GetFunction(n.keyword, args); fn != nil {
				return fn
			}
		}
		return value.NewFunctionDiyFunction(n.text, args)
	case "index":
		objFunc := n.children[0].toFunction()
		var keyFunc Function
		if n.keyIsParameter {
			keyFunc = value.NewFunctionParameter(n.children[1].text)
		} else {
			keyFunc = n.children[1].toFunction()
		}
		return value.NewFunctionGetJsonValue(objFunc, keyFunc)
	case "dotkey":
		return value.NewFunctionGetJsonValue(n.children[0].toFunction(), value.NewFunctionValueText(CreateString(n.text)))
	case "bracket":
		return n.children[0].toFunction()
	case "binary":
		return n.binaryFunction()
	case "unary":
		return flow.NewFunctionNOT(n.children[0].toFunction())
	case "percent":
		return mathbase.NewFunctionPercentage(n.childFunctions())
	case "ternary":
		return flow.NewFunctionIF(n.childFunctions())
	case "array":
		return value.NewFunctionArray(n.childFunctions())
	case "arrayjson":
		return value.NewFunctionArrayJson(n.childFunctions())
	case "arrayjsonitem":
		return value.NewFunctionArrayJsonItem(n.text, n.children[0].toFunction())
	}
	panic(&ParseError{Message: fmt.Sprintf("unsupported node '%s'", n.kind)})
}

func (n *astNode) childFunctions() []Function {
	result := make([]Function, len(n.children))
	for i, child := range n.children {
		result[i] = child.toFunction()
	}
	return result
}

func (n *astNode) binaryFunction() Function {
	args := n.childFunctions()
	switch n.op {
	case "OPMUL":
		return internals.GetFunction("MUL", args)
	case "OPDIV":
		return internals.GetFunction("DIV", args)
	case "OPMOD":
		return internals.GetFunction("MOD", args)
	case "OPADD":
		return internals.GetFunction("ADD", args)
	case "OPSUB":
		return internals.GetFunction("SUB", args)
	case "OPCAT":
		return internals.GetFunction("CONNECT", args)
	case "OPGT":
		return internals.GetFunction("GT", args)
	case "OPGE":
		return internals.GetFunction("GE", args)
	case "OPLT":
		return internals.GetFunction("LT", args)
	case "OPLE":
		return internals.GetFunction("LE", args)
	case "OPEQ":
		return internals.GetFunction("EQ", args)
	case "OPNE":
		return internals.GetFunction("NE", args)
	case "OPAND":
		return operator.NewFunctionAnd2(args)
	case "OPOR":
		return operator.NewFunctionOr2(args)
	}
	panic(&ParseError{Message: fmt.Sprintf("unsupported operator '%s'", n.op)})
}

var precedence = map[string]int{
	"OPOR": 2, "OPAND": 3,
	"OPNE": 4, "OPEQ": 4,
	"OPGT": 5, "OPGE": 5, "OPLT": 5, "OPLE": 5,
	"OPCAT": 6, "OPADD": 7, "OPSUB": 7,
	"OPMUL": 8, "OPDIV": 8, "OPMOD": 8,
}

// Parser 递归下降语法分析器。
type Parser struct {
	text   string
	runes  []rune
	tokens []Token
	pos    int
}

// NewParser 创建分析器。
func NewParser(text string) *Parser {
	return &Parser{text: text, runes: []rune(text), tokens: NewTokenizer(text).Tokenize()}
}

func (p *Parser) current() Token { return p.tokens[p.pos] }

func (p *Parser) peek(offset int) Token {
	index := p.pos + offset
	if index >= len(p.tokens) {
		index = len(p.tokens) - 1
	}
	return p.tokens[index]
}

func (p *Parser) advance() Token {
	token := p.tokens[p.pos]
	if token.Kind != TokEOF {
		p.pos++
	}
	return token
}

func (p *Parser) expect(kind string) Token {
	token := p.current()
	if token.Kind != kind {
		panic(&ParseError{Message: fmt.Sprintf("mismatched input '%s' expecting %s at position %d", token.Text, kind, token.Start)})
	}
	return p.advance()
}

// Parse 解析整个表达式。
func (p *Parser) Parse() *astNode {
	node := p.parseExpr(0)
	if p.current().Kind != TokEOF {
		token := p.current()
		panic(&ParseError{Message: fmt.Sprintf("extraneous input '%s' at position %d", token.Text, token.Start)})
	}
	return node
}

func (p *Parser) parseExpr(minPrecedence int) *astNode {
	left := p.parsePrefix()
	for {
		token := p.current()
		kind := token.Kind
		if kind == "QUESTION" && minPrecedence <= 1 {
			p.advance()
			whenTrue := p.parseExpr(0)
			p.expect("COLON")
			whenFalse := p.parseExpr(1)
			left = &astNode{kind: "ternary", start: left.start, end: whenFalse.end, children: []*astNode{left, whenTrue, whenFalse}}
			continue
		}
		if kind == "OPMOD" && minPrecedence <= 9 && !p.startsOperand(p.peek(1)) {
			p.advance()
			left = &astNode{kind: "percent", start: left.start, end: token.End, children: []*astNode{left}}
			continue
		}
		prec, ok := precedence[kind]
		if !ok || prec < minPrecedence {
			break
		}
		p.advance()
		right := p.parseExpr(prec + 1)
		left = &astNode{kind: "binary", start: left.start, end: right.end, op: kind, children: []*astNode{left, right}}
	}
	return left
}

func (p *Parser) startsOperand(token Token) bool {
	switch token.Kind {
	case "NUM", "STRING", "PARAMETER", "LPAREN", "LBRACKET", "LBRACE", "NOT", "OPADD", "OPSUB", "TRUE", "FALSE", "NULL":
		return true
	}
	if _, ok := Keywords[token.Kind]; ok {
		return true
	}
	return false
}

func (p *Parser) parsePrefix() *astNode {
	token := p.current()
	if token.Kind == "NOT" {
		p.advance()
		operand := p.parseExpr(9)
		return &astNode{kind: "unary", start: token.Start, end: operand.end, children: []*astNode{operand}}
	}
	if token.Kind == "OPSUB" && p.peek(1).Kind == "NUM" {
		sign := p.advance()
		number := p.advance()
		return p.numberNode(sign.Start, number)
	}
	node := p.parsePrimary()
	return p.parsePostfix(node)
}

func (p *Parser) parsePrimary() *astNode {
	token := p.current()
	switch token.Kind {
	case "NUM":
		p.advance()
		return p.numberNode(token.Start, token)
	case "STRING":
		p.advance()
		return &astNode{kind: "string", start: token.Start, end: token.End, text: unescapeText(token.Text)}
	case "LPAREN":
		p.advance()
		inner := p.parseExpr(0)
		close := p.expect("RPAREN")
		return &astNode{kind: "bracket", start: token.Start, end: close.End, children: []*astNode{inner}}
	case "LBRACKET":
		p.advance()
		items := []*astNode{}
		if p.current().Kind != "RBRACKET" {
			items = append(items, p.parseExpr(0))
			for p.current().Kind == "COMMA" {
				p.advance()
				if p.current().Kind == "RBRACKET" {
					break
				}
				items = append(items, p.parseExpr(0))
			}
		}
		close := p.expect("RBRACKET")
		return &astNode{kind: "array", start: token.Start, end: close.End, children: items}
	case "LBRACE":
		p.advance()
		items := []*astNode{}
		for p.current().Kind != "RBRACE" {
			items = append(items, p.parseArrayJsonItem())
			if p.current().Kind == "COMMA" {
				p.advance()
				continue
			}
			break
		}
		close := p.expect("RBRACE")
		return &astNode{kind: "arrayjson", start: token.Start, end: close.End, children: items}
	case "PARAMETER":
		p.advance()
		if p.current().Kind == "LPAREN" {
			args, close := p.parseArguments()
			return &astNode{kind: "call", start: token.Start, end: close.End, text: token.Text, keyword: "PARAMETER", children: args}
		}
		return &astNode{kind: "param", start: token.Start, end: token.End, text: token.Text}
	case "TRUE", "FALSE", "NULL", "ALGORITHMVERSION":
		p.advance()
		if p.current().Kind == "LPAREN" && p.peek(1).Kind == "RPAREN" {
			p.advance()
			p.advance()
		}
		return &astNode{kind: "const2", start: token.Start, end: token.End, keyword: token.Kind}
	case "E", "PI", "RAND", "GUID", "NOW", "TODAY":
		p.advance()
		if p.current().Kind == "LPAREN" && p.peek(1).Kind == "RPAREN" {
			p.advance()
			p.advance()
			return &astNode{kind: "const", start: token.Start, end: p.tokens[p.pos-1].End, keyword: token.Kind}
		}
		return &astNode{kind: "call", start: token.Start, end: token.End, text: token.Text, keyword: token.Kind}
	}
	if _, ok := Keywords[token.Kind]; ok {
		keyword := token.Kind
		p.advance()
		if p.current().Kind == "LPAREN" {
			args, close := p.parseArguments()
			return &astNode{kind: "call", start: token.Start, end: close.End, text: token.Text, keyword: keyword, children: args}
		}
		switch keyword {
		case "TRUE", "FALSE", "NULL", "ALGORITHMVERSION":
			return &astNode{kind: "const2", start: token.Start, end: token.End, keyword: keyword}
		case "E", "PI", "RAND", "GUID", "NOW", "TODAY":
			return &astNode{kind: "const", start: token.Start, end: token.End, keyword: keyword}
		}
		panic(&ParseError{Message: fmt.Sprintf("mismatched input '%s' expecting '(' at position %d", token.Text, token.Start)})
	}
	display := token.Text
	if display == "" {
		display = "<EOF>"
	}
	panic(&ParseError{Message: fmt.Sprintf("mismatched input '%s' at position %d", display, token.Start)})
}

func (p *Parser) parsePostfix(node *astNode) *astNode {
	for {
		token := p.current()
		if token.Kind == "DOT" {
			p.advance()
			name := p.current()
			_, isKeyword := Keywords[name.Kind]
			if name.Kind != "PARAMETER" && !isKeyword {
				panic(&ParseError{Message: fmt.Sprintf("unknown property '%s' at position %d", name.Text, name.Start)})
			}
			p.advance()
			if p.current().Kind == "LPAREN" {
				args, close := p.parseArguments()
				keyword := name.Kind
				if name.Kind == "PARAMETER" {
					keyword = "PARAMETER"
				}
				children := append([]*astNode{node}, args...)
				node = &astNode{kind: "method", start: node.start, end: close.End, text: name.Text, keyword: keyword, children: children}
			} else {
				node = &astNode{kind: "dotkey", start: node.start, end: name.End, text: name.Text, children: []*astNode{node}}
			}
			continue
		}
		if token.Kind == "LBRACKET" {
			p.advance()
			keyIsParameter := p.current().Kind == "PARAMETER" && p.peek(1).Kind == "RBRACKET"
			key := p.parseExpr(0)
			close := p.expect("RBRACKET")
			node = &astNode{kind: "index", start: node.start, end: close.End, children: []*astNode{node, key}, keyIsParameter: keyIsParameter}
			continue
		}
		break
	}
	return node
}

func (p *Parser) numberNode(start int, token Token) *astNode {
	text := token.Text
	if start != token.Start {
		text = string(p.runes[start : token.End+1])
	}
	if d, ok := TryParseDecimal(text); ok {
		if d.Abs().Greater(DecimalMax) {
			panic(&ParseError{Message: fmt.Sprintf("Number '%s' is invalid or out of range.", text)})
		}
		return &astNode{kind: "num", start: start, end: token.End, value: d}
	}
	body, unit := splitNumberUnit(text)
	d, ok := TryParseDecimal(body)
	if !ok || d.Abs().Greater(DecimalMax) {
		panic(&ParseError{Message: fmt.Sprintf("Number '%s' is invalid or out of range.", text)})
	}
	return &astNode{kind: "num", start: start, end: token.End, value: d, unit: unit, hasUnit: true}
}

func (p *Parser) parseArguments() ([]*astNode, Token) {
	p.expect("LPAREN")
	args := []*astNode{}
	if p.current().Kind != "RPAREN" {
		args = append(args, p.parseExpr(0))
		for p.current().Kind == "COMMA" {
			p.advance()
			args = append(args, p.parseExpr(0))
		}
	}
	close := p.expect("RPAREN")
	return args, close
}

func (p *Parser) parseArrayJsonItem() *astNode {
	token := p.current()
	if token.Kind == "NUM" || token.Kind == "STRING" || (token.Kind == "PARAMETER" && p.peek(1).Kind == "COLON") {
		p.advance()
		var key string
		if token.Kind == "STRING" {
			key = unescapeText(token.Text)
		} else {
			key = strings.Trim(token.Text, "'\"` ")
		}
		p.expect("COLON")
		value := p.parseExpr(0)
		return &astNode{kind: "arrayjsonitem", start: token.Start, end: value.end, text: key, children: []*astNode{value}}
	}
	if _, ok := Keywords[token.Kind]; ok {
		p.advance()
		if p.current().Kind == "COLON" {
			p.expect("COLON")
			value := p.parseExpr(0)
			return &astNode{kind: "arrayjsonitem", start: token.Start, end: value.end, text: token.Text, children: []*astNode{value}}
		}
		p.pos--
	}
	value := p.parseExpr(0)
	return &astNode{kind: "arrayjsonitem", start: value.start, end: value.end, children: []*astNode{value}}
}

func splitNumberUnit(text string) (string, string) {
	runes := []rune(text)
	n := len(runes)
	if n > 3 && runes[n-3] >= 'A' {
		return string(runes[:n-3]), string(runes[n-3:])
	}
	if n > 2 && runes[n-2] >= 'A' {
		return string(runes[:n-2]), string(runes[n-2:])
	}
	if n <= 1 {
		return text, ""
	}
	return string(runes[:n-1]), string(runes[n-1:])
}

var escapeMap = map[rune]string{
	'n': "\n", 'r': "\r", 't': "\t", '0': "\x00", 'v': "\v", 'a': "\a", 'b': "\b", 'f': "\f",
}

func unescapeText(text string) string {
	runes := []rune(text)
	if len(runes) < 2 {
		return text
	}
	body := runes[1 : len(runes)-1]
	var sb strings.Builder
	for i := 0; i < len(body); i++ {
		c := body[i]
		if c == '\\' && i+1 < len(body) {
			nxt := body[i+1]
			if nxt == 'u' && i+5 < len(body) {
				var code int
				if _, err := fmt.Sscanf(string(body[i+2:i+6]), "%04x", &code); err == nil {
					sb.WriteRune(rune(code))
					i += 5
					continue
				}
			}
			if mapped, ok := escapeMap[nxt]; ok {
				sb.WriteString(mapped)
			} else {
				sb.WriteRune(nxt)
			}
			i++
			continue
		}
		sb.WriteRune(c)
	}
	return sb.String()
}

// parseFunction 解析表达式并生成可执行节点。
func parseFunction(text string) (fn Function, err error) {
	defer func() {
		if r := recover(); r != nil {
			switch e := r.(type) {
			case *ParseError:
				err = e
			case error:
				err = &ParseError{Message: e.Error()}
			default:
				err = &ParseError{Message: fmt.Sprint(r)}
			}
			fn = nil
		}
	}()
	node := NewParser(text).Parse()
	return node.toFunction(), nil
}

// ParseFunction 解析表达式，返回节点或错误。
func ParseFunction(text string) (Function, error) { return parseFunction(text) }
