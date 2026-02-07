class FsmContext {
    constructor() {
        this.ret = false;
        this.nextState = 0;
        this.L = null;
        this.stateStack = 0;
    }
}

export { FsmContext };