var ApiCallResult = /** @class */ (function () {
    function ApiCallResult() {
    }
    ApiCallResult.prototype.withSuccess = function (data, status) {
        if (status === void 0) { status = 200; }
        this.ok = true;
        this.status = 200;
        this.data = data;
        return this;
    };
    ApiCallResult.prototype.withError = function (message, status) {
        this.ok = false;
        this.status = status;
        this.data = undefined;
        this.message = message;
        return this;
    };
    return ApiCallResult;
}());
//# sourceMappingURL=api-call-result.js.map