class FsmContext {
    constructor() {
        this.Return = false;
        this.NextState = 0;
        this.L = null;
        this.StateStack = 0;
    }
}

export { FsmContext };