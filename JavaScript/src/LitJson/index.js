import { FsmContext } from './FsmContext.js';
import { IJsonWrapper } from './IJsonWrapper.js';
import { JsonData } from './JsonData.js';
import { JsonException } from './JsonException.js';
import { JsonMapper } from './JsonMapper.js';
import { JsonReader } from './JsonReader.js';
import { JsonType } from './JsonType.js';
import { Lexer } from './Lexer.js';
import { ParserToken } from './ParserToken.js';
import { StringReader } from './StringReader.js';

export { FsmContext, IJsonWrapper, JsonData, JsonException, JsonMapper, JsonReader, JsonType, Lexer, ParserToken, StringReader };

// 浏览器支持
if (typeof window !== 'undefined') {
    window.LitJson = window.LitJson || {};
    window.LitJson.FsmContext = FsmContext;
    window.LitJson.IJsonWrapper = IJsonWrapper;
    window.LitJson.JsonData = JsonData;
    window.LitJson.JsonException = JsonException;
    window.LitJson.JsonMapper = JsonMapper;
    window.LitJson.JsonReader = JsonReader;
    window.LitJson.JsonType = JsonType;
    window.LitJson.Lexer = Lexer;
    window.LitJson.ParserToken = ParserToken;
    window.LitJson.StringReader = StringReader;
}