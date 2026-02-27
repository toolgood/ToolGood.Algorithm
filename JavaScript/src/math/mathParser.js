// Generated from math.g4 by ANTLR 4.13.2
// jshint ignore: start
import antlr4 from '../antlr4/index.web.js';
import mathVisitor from './mathVisitor.js';

const serializedATN = [4,1,38,142,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,1,0,1,
0,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,1,24,8,1,10,1,12,
1,27,9,1,3,1,29,8,1,1,1,1,1,1,1,1,1,1,1,5,1,36,8,1,10,1,12,1,39,9,1,1,1,
5,1,42,8,1,10,1,12,1,45,9,1,1,1,1,1,1,1,1,1,1,1,1,1,5,1,53,8,1,10,1,12,1,
56,9,1,1,1,5,1,59,8,1,10,1,12,1,62,9,1,1,1,1,1,1,1,1,1,1,1,3,1,69,8,1,1,
1,1,1,3,1,73,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,1,
106,8,1,10,1,12,1,109,9,1,3,1,111,8,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
1,1,1,1,1,1,1,1,1,1,1,1,1,5,1,128,8,1,10,1,12,1,131,9,1,1,2,3,2,134,8,2,
1,2,1,2,1,3,1,3,1,3,1,3,1,3,0,1,2,4,0,2,4,6,0,7,1,0,33,35,1,0,33,34,1,0,
8,10,1,0,11,13,1,0,14,17,1,0,18,23,1,0,30,35,167,0,8,1,0,0,0,2,72,1,0,0,
0,4,133,1,0,0,0,6,137,1,0,0,0,8,9,3,2,1,0,9,10,5,0,0,1,10,1,1,0,0,0,11,12,
6,1,-1,0,12,13,5,2,0,0,13,14,3,2,1,0,14,15,5,4,0,0,15,73,1,0,0,0,16,17,5,
7,0,0,17,73,3,2,1,16,18,19,7,0,0,0,19,28,5,2,0,0,20,25,3,2,1,0,21,22,5,3,
0,0,22,24,3,2,1,0,23,21,1,0,0,0,24,27,1,0,0,0,25,23,1,0,0,0,25,26,1,0,0,
0,26,29,1,0,0,0,27,25,1,0,0,0,28,20,1,0,0,0,28,29,1,0,0,0,29,30,1,0,0,0,
30,73,5,4,0,0,31,32,5,28,0,0,32,37,3,6,3,0,33,34,5,3,0,0,34,36,3,6,3,0,35,
33,1,0,0,0,36,39,1,0,0,0,37,35,1,0,0,0,37,38,1,0,0,0,38,43,1,0,0,0,39,37,
1,0,0,0,40,42,5,3,0,0,41,40,1,0,0,0,42,45,1,0,0,0,43,41,1,0,0,0,43,44,1,
0,0,0,44,46,1,0,0,0,45,43,1,0,0,0,46,47,5,29,0,0,47,73,1,0,0,0,48,49,5,5,
0,0,49,54,3,2,1,0,50,51,5,3,0,0,51,53,3,2,1,0,52,50,1,0,0,0,53,56,1,0,0,
0,54,52,1,0,0,0,54,55,1,0,0,0,55,60,1,0,0,0,56,54,1,0,0,0,57,59,5,3,0,0,
58,57,1,0,0,0,59,62,1,0,0,0,60,58,1,0,0,0,60,61,1,0,0,0,61,63,1,0,0,0,62,
60,1,0,0,0,63,64,5,6,0,0,64,73,1,0,0,0,65,73,5,35,0,0,66,68,3,4,2,0,67,69,
7,1,0,0,68,67,1,0,0,0,68,69,1,0,0,0,69,73,1,0,0,0,70,73,5,31,0,0,71,73,5,
32,0,0,72,11,1,0,0,0,72,16,1,0,0,0,72,18,1,0,0,0,72,31,1,0,0,0,72,48,1,0,
0,0,72,65,1,0,0,0,72,66,1,0,0,0,72,70,1,0,0,0,72,71,1,0,0,0,73,129,1,0,0,
0,74,75,10,14,0,0,75,76,7,2,0,0,76,128,3,2,1,15,77,78,10,13,0,0,78,79,7,
3,0,0,79,128,3,2,1,14,80,81,10,12,0,0,81,82,7,4,0,0,82,128,3,2,1,13,83,84,
10,11,0,0,84,85,7,5,0,0,85,128,3,2,1,12,86,87,10,10,0,0,87,88,5,24,0,0,88,
128,3,2,1,11,89,90,10,9,0,0,90,91,5,25,0,0,91,128,3,2,1,10,92,93,10,8,0,
0,93,94,5,26,0,0,94,95,3,2,1,0,95,96,5,27,0,0,96,97,3,2,1,9,97,128,1,0,0,
0,98,99,10,21,0,0,99,100,5,1,0,0,100,101,7,0,0,0,101,110,5,2,0,0,102,107,
3,2,1,0,103,104,5,3,0,0,104,106,3,2,1,0,105,103,1,0,0,0,106,109,1,0,0,0,
107,105,1,0,0,0,107,108,1,0,0,0,108,111,1,0,0,0,109,107,1,0,0,0,110,102,
1,0,0,0,110,111,1,0,0,0,111,112,1,0,0,0,112,128,5,4,0,0,113,114,10,20,0,
0,114,115,5,5,0,0,115,116,5,35,0,0,116,128,5,6,0,0,117,118,10,19,0,0,118,
119,5,5,0,0,119,120,3,2,1,0,120,121,5,6,0,0,121,128,1,0,0,0,122,123,10,18,
0,0,123,124,5,1,0,0,124,128,7,0,0,0,125,126,10,15,0,0,126,128,5,8,0,0,127,
74,1,0,0,0,127,77,1,0,0,0,127,80,1,0,0,0,127,83,1,0,0,0,127,86,1,0,0,0,127,
89,1,0,0,0,127,92,1,0,0,0,127,98,1,0,0,0,127,113,1,0,0,0,127,117,1,0,0,0,
127,122,1,0,0,0,127,125,1,0,0,0,128,131,1,0,0,0,129,127,1,0,0,0,129,130,
1,0,0,0,130,3,1,0,0,0,131,129,1,0,0,0,132,134,5,12,0,0,133,132,1,0,0,0,133,
134,1,0,0,0,134,135,1,0,0,0,135,136,5,30,0,0,136,5,1,0,0,0,137,138,7,6,0,
0,138,139,5,27,0,0,139,140,3,2,1,0,140,7,1,0,0,0,13,25,28,37,43,54,60,68,
72,107,110,127,129,133];


const atn = new antlr4.atn.ATNDeserializer().deserialize(serializedATN);

const decisionsToDFA = atn.decisionToState.map( (ds, index) => new antlr4.dfa.DFA(ds, index) );

const sharedContextCache = new antlr4.atn.PredictionContextCache();

export default class mathParser extends antlr4.Parser {

    static grammarFileName = "math.g4";
    static literalNames = [ null, "'.'", "'('", "','", "')'", "'['", "']'", 
                            "'!'", "'%'", "'*'", "'/'", "'+'", "'-'", "'&'", 
                            "'>'", "'>='", "'<'", "'<='", "'='", "'=='", 
                            "'==='", "'!=='", "'!='", "'<>'", "'&&'", "'||'", 
                            "'?'", "':'", "'{'", "'}'", null, null, "'NULL'", 
                            null, "'T'" ];
    static symbolicNames = [ null, null, null, null, null, null, null, null, 
                             null, null, null, null, null, null, null, null, 
                             null, null, null, null, null, null, null, null, 
                             null, null, null, null, null, null, "NUM", 
                             "STRING", "NULL", "UNIT", "T", "PARAMETER", 
                             "WS", "COMMENT", "LINE_COMMENT" ];
    static ruleNames = [ "prog", "expr", "num", "arrayJson" ];

    constructor(input) {
        super(input);
        this._interp = new antlr4.atn.ParserATNSimulator(this, atn, decisionsToDFA, sharedContextCache);
        this.ruleNames = mathParser.ruleNames;
        this.literalNames = mathParser.literalNames;
        this.symbolicNames = mathParser.symbolicNames;
    }

    sempred(localctx, ruleIndex, predIndex) {
    	switch(ruleIndex) {
    	case 1:
    	    		return this.expr_sempred(localctx, predIndex);
        default:
            throw "No predicate with index:" + ruleIndex;
       }
    }

    expr_sempred(localctx, predIndex) {
    	switch(predIndex) {
    		case 0:
    			return this.precpred(this._ctx, 14);
    		case 1:
    			return this.precpred(this._ctx, 13);
    		case 2:
    			return this.precpred(this._ctx, 12);
    		case 3:
    			return this.precpred(this._ctx, 11);
    		case 4:
    			return this.precpred(this._ctx, 10);
    		case 5:
    			return this.precpred(this._ctx, 9);
    		case 6:
    			return this.precpred(this._ctx, 8);
    		case 7:
    			return this.precpred(this._ctx, 21);
    		case 8:
    			return this.precpred(this._ctx, 20);
    		case 9:
    			return this.precpred(this._ctx, 19);
    		case 10:
    			return this.precpred(this._ctx, 18);
    		case 11:
    			return this.precpred(this._ctx, 15);
    		default:
    			throw "No predicate with index:" + predIndex;
    	}
    };




	prog() {
	    let localctx = new ProgContext(this, this._ctx, this.state);
	    this.enterRule(localctx, 0, mathParser.RULE_prog);
	    try {
	        this.enterOuterAlt(localctx, 1);
	        this.state = 8;
	        this.expr(0);
	        this.state = 9;
	        this.match(mathParser.EOF);
	    } catch (re) {
	    	if(re instanceof antlr4.error.RecognitionException) {
		        localctx.exception = re;
		        this._errHandler.reportError(this, re);
		        this._errHandler.recover(this, re);
		    } else {
		    	throw re;
		    }
	    } finally {
	        this.exitRule();
	    }
	    return localctx;
	}


	expr(_p) {
		if(_p===undefined) {
		    _p = 0;
		}
	    const _parentctx = this._ctx;
	    const _parentState = this.state;
	    let localctx = new ExprContext(this, this._ctx, _parentState);
	    let _prevctx = localctx;
	    const _startState = 2;
	    this.enterRecursionRule(localctx, 2, mathParser.RULE_expr, _p);
	    var _la = 0;
	    try {
	        this.enterOuterAlt(localctx, 1);
	        this.state = 72;
	        this._errHandler.sync(this);
	        var la_ = this._interp.adaptivePredict(this._input,7,this._ctx);
	        switch(la_) {
	        case 1:
	            localctx = new Bracket_funContext(this, localctx);
	            this._ctx = localctx;
	            _prevctx = localctx;

	            this.state = 12;
	            this.match(mathParser.T__1);
	            this.state = 13;
	            this.expr(0);
	            this.state = 14;
	            this.match(mathParser.T__3);
	            break;

	        case 2:
	            localctx = new NOT_funContext(this, localctx);
	            this._ctx = localctx;
	            _prevctx = localctx;
	            this.state = 16;
	            this.match(mathParser.T__6);
	            this.state = 17;
	            this.expr(16);
	            break;

	        case 3:
	            localctx = new DiyFunction_funContext(this, localctx);
	            this._ctx = localctx;
	            _prevctx = localctx;
	            this.state = 18;
	            localctx.f = this._input.LT(1);
	            _la = this._input.LA(1);
	            if(!(((((_la - 33)) & ~0x1f) === 0 && ((1 << (_la - 33)) & 7) !== 0))) {
	                localctx.f = this._errHandler.recoverInline(this);
	            }
	            else {
	            	this._errHandler.reportMatch(this);
	                this.consume();
	            }
	            this.state = 19;
	            this.match(mathParser.T__1);
	            this.state = 28;
	            this._errHandler.sync(this);
	            _la = this._input.LA(1);
	            if((((_la) & ~0x1f) === 0 && ((1 << _la) & 3489665188) !== 0) || ((((_la - 32)) & ~0x1f) === 0 && ((1 << (_la - 32)) & 15) !== 0)) {
	                this.state = 20;
	                this.expr(0);
	                this.state = 25;
	                this._errHandler.sync(this);
	                _la = this._input.LA(1);
	                while(_la===3) {
	                    this.state = 21;
	                    this.match(mathParser.T__2);
	                    this.state = 22;
	                    this.expr(0);
	                    this.state = 27;
	                    this._errHandler.sync(this);
	                    _la = this._input.LA(1);
	                }
	            }

	            this.state = 30;
	            this.match(mathParser.T__3);
	            break;

	        case 4:
	            localctx = new ArrayJson_funContext(this, localctx);
	            this._ctx = localctx;
	            _prevctx = localctx;
	            this.state = 31;
	            this.match(mathParser.T__27);
	            this.state = 32;
	            this.arrayJson();
	            this.state = 37;
	            this._errHandler.sync(this);
	            var _alt = this._interp.adaptivePredict(this._input,2,this._ctx)
	            while(_alt!=2 && _alt!=antlr4.atn.ATN.INVALID_ALT_NUMBER) {
	                if(_alt===1) {
	                    this.state = 33;
	                    this.match(mathParser.T__2);
	                    this.state = 34;
	                    this.arrayJson(); 
	                }
	                this.state = 39;
	                this._errHandler.sync(this);
	                _alt = this._interp.adaptivePredict(this._input,2,this._ctx);
	            }

	            this.state = 43;
	            this._errHandler.sync(this);
	            _la = this._input.LA(1);
	            while(_la===3) {
	                this.state = 40;
	                this.match(mathParser.T__2);
	                this.state = 45;
	                this._errHandler.sync(this);
	                _la = this._input.LA(1);
	            }
	            this.state = 46;
	            this.match(mathParser.T__28);
	            break;

	        case 5:
	            localctx = new Array_funContext(this, localctx);
	            this._ctx = localctx;
	            _prevctx = localctx;
	            this.state = 48;
	            this.match(mathParser.T__4);
	            this.state = 49;
	            this.expr(0);
	            this.state = 54;
	            this._errHandler.sync(this);
	            var _alt = this._interp.adaptivePredict(this._input,4,this._ctx)
	            while(_alt!=2 && _alt!=antlr4.atn.ATN.INVALID_ALT_NUMBER) {
	                if(_alt===1) {
	                    this.state = 50;
	                    this.match(mathParser.T__2);
	                    this.state = 51;
	                    this.expr(0); 
	                }
	                this.state = 56;
	                this._errHandler.sync(this);
	                _alt = this._interp.adaptivePredict(this._input,4,this._ctx);
	            }

	            this.state = 60;
	            this._errHandler.sync(this);
	            _la = this._input.LA(1);
	            while(_la===3) {
	                this.state = 57;
	                this.match(mathParser.T__2);
	                this.state = 62;
	                this._errHandler.sync(this);
	                _la = this._input.LA(1);
	            }
	            this.state = 63;
	            this.match(mathParser.T__5);
	            break;

	        case 6:
	            localctx = new PARAMETER_funContext(this, localctx);
	            this._ctx = localctx;
	            _prevctx = localctx;
	            this.state = 65;
	            this.match(mathParser.PARAMETER);
	            break;

	        case 7:
	            localctx = new NUM_funContext(this, localctx);
	            this._ctx = localctx;
	            _prevctx = localctx;
	            this.state = 66;
	            this.num();
	            this.state = 68;
	            this._errHandler.sync(this);
	            var la_ = this._interp.adaptivePredict(this._input,6,this._ctx);
	            if(la_===1) {
	                this.state = 67;
	                localctx.unit = this._input.LT(1);
	                _la = this._input.LA(1);
	                if(!(_la===33 || _la===34)) {
	                    localctx.unit = this._errHandler.recoverInline(this);
	                }
	                else {
	                	this._errHandler.reportMatch(this);
	                    this.consume();
	                }

	            }
	            break;

	        case 8:
	            localctx = new STRING_funContext(this, localctx);
	            this._ctx = localctx;
	            _prevctx = localctx;
	            this.state = 70;
	            this.match(mathParser.STRING);
	            break;

	        case 9:
	            localctx = new NULL_funContext(this, localctx);
	            this._ctx = localctx;
	            _prevctx = localctx;
	            this.state = 71;
	            this.match(mathParser.NULL);
	            break;

	        }
	        this._ctx.stop = this._input.LT(-1);
	        this.state = 129;
	        this._errHandler.sync(this);
	        var _alt = this._interp.adaptivePredict(this._input,11,this._ctx)
	        while(_alt!=2 && _alt!=antlr4.atn.ATN.INVALID_ALT_NUMBER) {
	            if(_alt===1) {
	                if(this._parseListeners!==null) {
	                    this.triggerExitRuleEvent();
	                }
	                _prevctx = localctx;
	                this.state = 127;
	                this._errHandler.sync(this);
	                var la_ = this._interp.adaptivePredict(this._input,10,this._ctx);
	                switch(la_) {
	                case 1:
	                    localctx = new MulDiv_funContext(this, new ExprContext(this, _parentctx, _parentState));
	                    this.pushNewRecursionContext(localctx, _startState, mathParser.RULE_expr);
	                    this.state = 74;
	                    if (!( this.precpred(this._ctx, 14))) {
	                        throw new antlr4.error.FailedPredicateException(this, "this.precpred(this._ctx, 14)");
	                    }
	                    this.state = 75;
	                    localctx.op = this._input.LT(1);
	                    _la = this._input.LA(1);
	                    if(!((((_la) & ~0x1f) === 0 && ((1 << _la) & 1792) !== 0))) {
	                        localctx.op = this._errHandler.recoverInline(this);
	                    }
	                    else {
	                    	this._errHandler.reportMatch(this);
	                        this.consume();
	                    }
	                    this.state = 76;
	                    this.expr(15);
	                    break;

	                case 2:
	                    localctx = new AddSub_funContext(this, new ExprContext(this, _parentctx, _parentState));
	                    this.pushNewRecursionContext(localctx, _startState, mathParser.RULE_expr);
	                    this.state = 77;
	                    if (!( this.precpred(this._ctx, 13))) {
	                        throw new antlr4.error.FailedPredicateException(this, "this.precpred(this._ctx, 13)");
	                    }
	                    this.state = 78;
	                    localctx.op = this._input.LT(1);
	                    _la = this._input.LA(1);
	                    if(!((((_la) & ~0x1f) === 0 && ((1 << _la) & 14336) !== 0))) {
	                        localctx.op = this._errHandler.recoverInline(this);
	                    }
	                    else {
	                    	this._errHandler.reportMatch(this);
	                        this.consume();
	                    }
	                    this.state = 79;
	                    this.expr(14);
	                    break;

	                case 3:
	                    localctx = new Judge_funContext(this, new ExprContext(this, _parentctx, _parentState));
	                    this.pushNewRecursionContext(localctx, _startState, mathParser.RULE_expr);
	                    this.state = 80;
	                    if (!( this.precpred(this._ctx, 12))) {
	                        throw new antlr4.error.FailedPredicateException(this, "this.precpred(this._ctx, 12)");
	                    }
	                    this.state = 81;
	                    localctx.op = this._input.LT(1);
	                    _la = this._input.LA(1);
	                    if(!((((_la) & ~0x1f) === 0 && ((1 << _la) & 245760) !== 0))) {
	                        localctx.op = this._errHandler.recoverInline(this);
	                    }
	                    else {
	                    	this._errHandler.reportMatch(this);
	                        this.consume();
	                    }
	                    this.state = 82;
	                    this.expr(13);
	                    break;

	                case 4:
	                    localctx = new Judge_funContext(this, new ExprContext(this, _parentctx, _parentState));
	                    this.pushNewRecursionContext(localctx, _startState, mathParser.RULE_expr);
	                    this.state = 83;
	                    if (!( this.precpred(this._ctx, 11))) {
	                        throw new antlr4.error.FailedPredicateException(this, "this.precpred(this._ctx, 11)");
	                    }
	                    this.state = 84;
	                    localctx.op = this._input.LT(1);
	                    _la = this._input.LA(1);
	                    if(!((((_la) & ~0x1f) === 0 && ((1 << _la) & 16515072) !== 0))) {
	                        localctx.op = this._errHandler.recoverInline(this);
	                    }
	                    else {
	                    	this._errHandler.reportMatch(this);
	                        this.consume();
	                    }
	                    this.state = 85;
	                    this.expr(12);
	                    break;

	                case 5:
	                    localctx = new AndOr_funContext(this, new ExprContext(this, _parentctx, _parentState));
	                    this.pushNewRecursionContext(localctx, _startState, mathParser.RULE_expr);
	                    this.state = 86;
	                    if (!( this.precpred(this._ctx, 10))) {
	                        throw new antlr4.error.FailedPredicateException(this, "this.precpred(this._ctx, 10)");
	                    }
	                    this.state = 87;
	                    localctx.op = this.match(mathParser.T__23);
	                    this.state = 88;
	                    this.expr(11);
	                    break;

	                case 6:
	                    localctx = new AndOr_funContext(this, new ExprContext(this, _parentctx, _parentState));
	                    this.pushNewRecursionContext(localctx, _startState, mathParser.RULE_expr);
	                    this.state = 89;
	                    if (!( this.precpred(this._ctx, 9))) {
	                        throw new antlr4.error.FailedPredicateException(this, "this.precpred(this._ctx, 9)");
	                    }
	                    this.state = 90;
	                    localctx.op = this.match(mathParser.T__24);
	                    this.state = 91;
	                    this.expr(10);
	                    break;

	                case 7:
	                    localctx = new IF_funContext(this, new ExprContext(this, _parentctx, _parentState));
	                    this.pushNewRecursionContext(localctx, _startState, mathParser.RULE_expr);
	                    this.state = 92;
	                    if (!( this.precpred(this._ctx, 8))) {
	                        throw new antlr4.error.FailedPredicateException(this, "this.precpred(this._ctx, 8)");
	                    }
	                    this.state = 93;
	                    this.match(mathParser.T__25);
	                    this.state = 94;
	                    this.expr(0);
	                    this.state = 95;
	                    this.match(mathParser.T__26);
	                    this.state = 96;
	                    this.expr(9);
	                    break;

	                case 8:
	                    localctx = new DiyFunction_funContext(this, new ExprContext(this, _parentctx, _parentState));
	                    this.pushNewRecursionContext(localctx, _startState, mathParser.RULE_expr);
	                    this.state = 98;
	                    if (!( this.precpred(this._ctx, 21))) {
	                        throw new antlr4.error.FailedPredicateException(this, "this.precpred(this._ctx, 21)");
	                    }
	                    this.state = 99;
	                    this.match(mathParser.T__0);
	                    this.state = 100;
	                    localctx.f = this._input.LT(1);
	                    _la = this._input.LA(1);
	                    if(!(((((_la - 33)) & ~0x1f) === 0 && ((1 << (_la - 33)) & 7) !== 0))) {
	                        localctx.f = this._errHandler.recoverInline(this);
	                    }
	                    else {
	                    	this._errHandler.reportMatch(this);
	                        this.consume();
	                    }
	                    this.state = 101;
	                    this.match(mathParser.T__1);
	                    this.state = 110;
	                    this._errHandler.sync(this);
	                    _la = this._input.LA(1);
	                    if((((_la) & ~0x1f) === 0 && ((1 << _la) & 3489665188) !== 0) || ((((_la - 32)) & ~0x1f) === 0 && ((1 << (_la - 32)) & 15) !== 0)) {
	                        this.state = 102;
	                        this.expr(0);
	                        this.state = 107;
	                        this._errHandler.sync(this);
	                        _la = this._input.LA(1);
	                        while(_la===3) {
	                            this.state = 103;
	                            this.match(mathParser.T__2);
	                            this.state = 104;
	                            this.expr(0);
	                            this.state = 109;
	                            this._errHandler.sync(this);
	                            _la = this._input.LA(1);
	                        }
	                    }

	                    this.state = 112;
	                    this.match(mathParser.T__3);
	                    break;

	                case 9:
	                    localctx = new GetJsonValue_funContext(this, new ExprContext(this, _parentctx, _parentState));
	                    this.pushNewRecursionContext(localctx, _startState, mathParser.RULE_expr);
	                    this.state = 113;
	                    if (!( this.precpred(this._ctx, 20))) {
	                        throw new antlr4.error.FailedPredicateException(this, "this.precpred(this._ctx, 20)");
	                    }
	                    this.state = 114;
	                    this.match(mathParser.T__4);
	                    this.state = 115;
	                    this.match(mathParser.PARAMETER);
	                    this.state = 116;
	                    this.match(mathParser.T__5);
	                    break;

	                case 10:
	                    localctx = new GetJsonValue_funContext(this, new ExprContext(this, _parentctx, _parentState));
	                    this.pushNewRecursionContext(localctx, _startState, mathParser.RULE_expr);
	                    this.state = 117;
	                    if (!( this.precpred(this._ctx, 19))) {
	                        throw new antlr4.error.FailedPredicateException(this, "this.precpred(this._ctx, 19)");
	                    }
	                    this.state = 118;
	                    this.match(mathParser.T__4);
	                    this.state = 119;
	                    this.expr(0);
	                    this.state = 120;
	                    this.match(mathParser.T__5);
	                    break;

	                case 11:
	                    localctx = new GetJsonValue_funContext(this, new ExprContext(this, _parentctx, _parentState));
	                    this.pushNewRecursionContext(localctx, _startState, mathParser.RULE_expr);
	                    this.state = 122;
	                    if (!( this.precpred(this._ctx, 18))) {
	                        throw new antlr4.error.FailedPredicateException(this, "this.precpred(this._ctx, 18)");
	                    }
	                    this.state = 123;
	                    this.match(mathParser.T__0);
	                    this.state = 124;
	                    localctx.p = this._input.LT(1);
	                    _la = this._input.LA(1);
	                    if(!(((((_la - 33)) & ~0x1f) === 0 && ((1 << (_la - 33)) & 7) !== 0))) {
	                        localctx.p = this._errHandler.recoverInline(this);
	                    }
	                    else {
	                    	this._errHandler.reportMatch(this);
	                        this.consume();
	                    }
	                    break;

	                case 12:
	                    localctx = new Percentage_funContext(this, new ExprContext(this, _parentctx, _parentState));
	                    this.pushNewRecursionContext(localctx, _startState, mathParser.RULE_expr);
	                    this.state = 125;
	                    if (!( this.precpred(this._ctx, 15))) {
	                        throw new antlr4.error.FailedPredicateException(this, "this.precpred(this._ctx, 15)");
	                    }
	                    this.state = 126;
	                    this.match(mathParser.T__7);
	                    break;

	                } 
	            }
	            this.state = 131;
	            this._errHandler.sync(this);
	            _alt = this._interp.adaptivePredict(this._input,11,this._ctx);
	        }

	    } catch( error) {
	        if(error instanceof antlr4.error.RecognitionException) {
		        localctx.exception = error;
		        this._errHandler.reportError(this, error);
		        this._errHandler.recover(this, error);
		    } else {
		    	throw error;
		    }
	    } finally {
	        this.unrollRecursionContexts(_parentctx)
	    }
	    return localctx;
	}



	num() {
	    let localctx = new NumContext(this, this._ctx, this.state);
	    this.enterRule(localctx, 4, mathParser.RULE_num);
	    var _la = 0;
	    try {
	        this.enterOuterAlt(localctx, 1);
	        this.state = 133;
	        this._errHandler.sync(this);
	        _la = this._input.LA(1);
	        if(_la===12) {
	            this.state = 132;
	            this.match(mathParser.T__11);
	        }

	        this.state = 135;
	        this.match(mathParser.NUM);
	    } catch (re) {
	    	if(re instanceof antlr4.error.RecognitionException) {
		        localctx.exception = re;
		        this._errHandler.reportError(this, re);
		        this._errHandler.recover(this, re);
		    } else {
		    	throw re;
		    }
	    } finally {
	        this.exitRule();
	    }
	    return localctx;
	}



	arrayJson() {
	    let localctx = new ArrayJsonContext(this, this._ctx, this.state);
	    this.enterRule(localctx, 6, mathParser.RULE_arrayJson);
	    var _la = 0;
	    try {
	        this.enterOuterAlt(localctx, 1);
	        this.state = 137;
	        localctx.key = this._input.LT(1);
	        _la = this._input.LA(1);
	        if(!(((((_la - 30)) & ~0x1f) === 0 && ((1 << (_la - 30)) & 63) !== 0))) {
	            localctx.key = this._errHandler.recoverInline(this);
	        }
	        else {
	        	this._errHandler.reportMatch(this);
	            this.consume();
	        }
	        this.state = 138;
	        this.match(mathParser.T__26);
	        this.state = 139;
	        this.expr(0);
	    } catch (re) {
	    	if(re instanceof antlr4.error.RecognitionException) {
		        localctx.exception = re;
		        this._errHandler.reportError(this, re);
		        this._errHandler.recover(this, re);
		    } else {
		    	throw re;
		    }
	    } finally {
	        this.exitRule();
	    }
	    return localctx;
	}


}

mathParser.EOF = antlr4.Token.EOF;
mathParser.T__0 = 1;
mathParser.T__1 = 2;
mathParser.T__2 = 3;
mathParser.T__3 = 4;
mathParser.T__4 = 5;
mathParser.T__5 = 6;
mathParser.T__6 = 7;
mathParser.T__7 = 8;
mathParser.T__8 = 9;
mathParser.T__9 = 10;
mathParser.T__10 = 11;
mathParser.T__11 = 12;
mathParser.T__12 = 13;
mathParser.T__13 = 14;
mathParser.T__14 = 15;
mathParser.T__15 = 16;
mathParser.T__16 = 17;
mathParser.T__17 = 18;
mathParser.T__18 = 19;
mathParser.T__19 = 20;
mathParser.T__20 = 21;
mathParser.T__21 = 22;
mathParser.T__22 = 23;
mathParser.T__23 = 24;
mathParser.T__24 = 25;
mathParser.T__25 = 26;
mathParser.T__26 = 27;
mathParser.T__27 = 28;
mathParser.T__28 = 29;
mathParser.NUM = 30;
mathParser.STRING = 31;
mathParser.NULL = 32;
mathParser.UNIT = 33;
mathParser.T = 34;
mathParser.PARAMETER = 35;
mathParser.WS = 36;
mathParser.COMMENT = 37;
mathParser.LINE_COMMENT = 38;

mathParser.RULE_prog = 0;
mathParser.RULE_expr = 1;
mathParser.RULE_num = 2;
mathParser.RULE_arrayJson = 3;

class ProgContext extends antlr4.ParserRuleContext {

    constructor(parser, parent, invokingState) {
        if(parent===undefined) {
            parent = null;
        }
        if(invokingState===undefined || invokingState===null) {
            invokingState = -1;
        }
        super(parent, invokingState);
        this.parser = parser;
        this.ruleIndex = mathParser.RULE_prog;
    }

	expr() {
	    return this.getTypedRuleContext(ExprContext,0);
	};

	EOF() {
	    return this.getToken(mathParser.EOF, 0);
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitProg(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}



class ExprContext extends antlr4.ParserRuleContext {

    constructor(parser, parent, invokingState) {
        if(parent===undefined) {
            parent = null;
        }
        if(invokingState===undefined || invokingState===null) {
            invokingState = -1;
        }
        super(parent, invokingState);
        this.parser = parser;
        this.ruleIndex = mathParser.RULE_expr;
    }


	 
		copyFrom(ctx) {
			super.copyFrom(ctx);
		}

}


class IF_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        super.copyFrom(ctx);
    }

	expr = function(i) {
	    if(i===undefined) {
	        i = null;
	    }
	    if(i===null) {
	        return this.getTypedRuleContexts(ExprContext);
	    } else {
	        return this.getTypedRuleContext(ExprContext,i);
	    }
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitIF_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.IF_funContext = IF_funContext;

class Judge_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        this.op = null;;
        super.copyFrom(ctx);
    }

	expr = function(i) {
	    if(i===undefined) {
	        i = null;
	    }
	    if(i===null) {
	        return this.getTypedRuleContexts(ExprContext);
	    } else {
	        return this.getTypedRuleContext(ExprContext,i);
	    }
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitJudge_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.Judge_funContext = Judge_funContext;

class AndOr_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        this.op = null;;
        super.copyFrom(ctx);
    }

	expr = function(i) {
	    if(i===undefined) {
	        i = null;
	    }
	    if(i===null) {
	        return this.getTypedRuleContexts(ExprContext);
	    } else {
	        return this.getTypedRuleContext(ExprContext,i);
	    }
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitAndOr_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.AndOr_funContext = AndOr_funContext;

class Percentage_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        super.copyFrom(ctx);
    }

	expr() {
	    return this.getTypedRuleContext(ExprContext,0);
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitPercentage_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.Percentage_funContext = Percentage_funContext;

class DiyFunction_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        this.f = null;;
        super.copyFrom(ctx);
    }

	T() {
	    return this.getToken(mathParser.T, 0);
	};

	UNIT() {
	    return this.getToken(mathParser.UNIT, 0);
	};

	PARAMETER() {
	    return this.getToken(mathParser.PARAMETER, 0);
	};

	expr = function(i) {
	    if(i===undefined) {
	        i = null;
	    }
	    if(i===null) {
	        return this.getTypedRuleContexts(ExprContext);
	    } else {
	        return this.getTypedRuleContext(ExprContext,i);
	    }
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitDiyFunction_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.DiyFunction_funContext = DiyFunction_funContext;

class STRING_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        super.copyFrom(ctx);
    }

	STRING() {
	    return this.getToken(mathParser.STRING, 0);
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitSTRING_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.STRING_funContext = STRING_funContext;

class AddSub_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        this.op = null;;
        super.copyFrom(ctx);
    }

	expr = function(i) {
	    if(i===undefined) {
	        i = null;
	    }
	    if(i===null) {
	        return this.getTypedRuleContexts(ExprContext);
	    } else {
	        return this.getTypedRuleContext(ExprContext,i);
	    }
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitAddSub_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.AddSub_funContext = AddSub_funContext;

class ArrayJson_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        super.copyFrom(ctx);
    }

	arrayJson = function(i) {
	    if(i===undefined) {
	        i = null;
	    }
	    if(i===null) {
	        return this.getTypedRuleContexts(ArrayJsonContext);
	    } else {
	        return this.getTypedRuleContext(ArrayJsonContext,i);
	    }
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitArrayJson_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.ArrayJson_funContext = ArrayJson_funContext;

class GetJsonValue_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        this.p = null;;
        super.copyFrom(ctx);
    }

	expr = function(i) {
	    if(i===undefined) {
	        i = null;
	    }
	    if(i===null) {
	        return this.getTypedRuleContexts(ExprContext);
	    } else {
	        return this.getTypedRuleContext(ExprContext,i);
	    }
	};

	PARAMETER() {
	    return this.getToken(mathParser.PARAMETER, 0);
	};

	T() {
	    return this.getToken(mathParser.T, 0);
	};

	UNIT() {
	    return this.getToken(mathParser.UNIT, 0);
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitGetJsonValue_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.GetJsonValue_funContext = GetJsonValue_funContext;

class Array_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        super.copyFrom(ctx);
    }

	expr = function(i) {
	    if(i===undefined) {
	        i = null;
	    }
	    if(i===null) {
	        return this.getTypedRuleContexts(ExprContext);
	    } else {
	        return this.getTypedRuleContext(ExprContext,i);
	    }
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitArray_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.Array_funContext = Array_funContext;

class MulDiv_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        this.op = null;;
        super.copyFrom(ctx);
    }

	expr = function(i) {
	    if(i===undefined) {
	        i = null;
	    }
	    if(i===null) {
	        return this.getTypedRuleContexts(ExprContext);
	    } else {
	        return this.getTypedRuleContext(ExprContext,i);
	    }
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitMulDiv_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.MulDiv_funContext = MulDiv_funContext;

class NOT_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        super.copyFrom(ctx);
    }

	expr() {
	    return this.getTypedRuleContext(ExprContext,0);
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitNOT_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.NOT_funContext = NOT_funContext;

class Bracket_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        super.copyFrom(ctx);
    }

	expr() {
	    return this.getTypedRuleContext(ExprContext,0);
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitBracket_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.Bracket_funContext = Bracket_funContext;

class PARAMETER_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        super.copyFrom(ctx);
    }

	PARAMETER() {
	    return this.getToken(mathParser.PARAMETER, 0);
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitPARAMETER_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.PARAMETER_funContext = PARAMETER_funContext;

class NUM_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        this.unit = null;;
        super.copyFrom(ctx);
    }

	num() {
	    return this.getTypedRuleContext(NumContext,0);
	};

	UNIT() {
	    return this.getToken(mathParser.UNIT, 0);
	};

	T() {
	    return this.getToken(mathParser.T, 0);
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitNUM_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.NUM_funContext = NUM_funContext;

class NULL_funContext extends ExprContext {

    constructor(parser, ctx) {
        super(parser);
        super.copyFrom(ctx);
    }

	NULL() {
	    return this.getToken(mathParser.NULL, 0);
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitNULL_fun(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}

mathParser.NULL_funContext = NULL_funContext;

class NumContext extends antlr4.ParserRuleContext {

    constructor(parser, parent, invokingState) {
        if(parent===undefined) {
            parent = null;
        }
        if(invokingState===undefined || invokingState===null) {
            invokingState = -1;
        }
        super(parent, invokingState);
        this.parser = parser;
        this.ruleIndex = mathParser.RULE_num;
    }

	NUM() {
	    return this.getToken(mathParser.NUM, 0);
	};

	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitNum(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}



class ArrayJsonContext extends antlr4.ParserRuleContext {

    constructor(parser, parent, invokingState) {
        if(parent===undefined) {
            parent = null;
        }
        if(invokingState===undefined || invokingState===null) {
            invokingState = -1;
        }
        super(parent, invokingState);
        this.parser = parser;
        this.ruleIndex = mathParser.RULE_arrayJson;
        this.key = null;
    }

	expr() {
	    return this.getTypedRuleContext(ExprContext,0);
	};
	accept(visitor) {
	    if ( visitor instanceof mathVisitor ) {
	        return visitor.visitArrayJson(this);
	    } else {
	        return visitor.visitChildren(this);
	    }
	}


}




mathParser.ProgContext = ProgContext; 
mathParser.ExprContext = ExprContext; 
mathParser.NumContext = NumContext; 
mathParser.ArrayJsonContext = ArrayJsonContext; 
