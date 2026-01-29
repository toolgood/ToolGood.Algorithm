import { JsonType } from './JsonType.js';

class IJsonWrapper {
    get IsArray() {
        throw new Error('Not implemented');
    }

    get IsBoolean() {
        throw new Error('Not implemented');
    }

    get IsDouble() {
        throw new Error('Not implemented');
    }

    get IsObject() {
        throw new Error('Not implemented');
    }

    get IsString() {
        throw new Error('Not implemented');
    }

    get IsNull() {
        throw new Error('Not implemented');
    }

    SetBoolean(val) {
        throw new Error('Not implemented');
    }

    SetDouble(val) {
        throw new Error('Not implemented');
    }

    SetJsonType(type) {
        throw new Error('Not implemented');
    }

    SetString(val) {
        throw new Error('Not implemented');
    }

    SetNull() {
        throw new Error('Not implemented');
    }

    Add(val) {
        throw new Error('Not implemented');
    }

    Set(key, val) {
        throw new Error('Not implemented');
    }
}

export { IJsonWrapper };