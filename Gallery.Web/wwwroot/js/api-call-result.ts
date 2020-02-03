class ApiCallResult {
    public data: any;
    public ok: boolean;
    public status: number;
    public message: string;

    public withSuccess(data: any, status: number = 200): ApiCallResult {
        this.ok = true;
        this.status = 200;
        this.data = data;
        return this;
    }

    public withError(message: string, status: number): ApiCallResult {
        this.ok = false;
        this.status = status;
        this.data = undefined;
        this.message = message;
        return this;
    }
}