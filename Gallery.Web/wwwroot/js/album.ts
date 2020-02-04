class Album {
    private readonly settings: Settings;
    private readonly service: Service;

    private getDisplayOrderSpan(processedFileName: string): HTMLElement {
        return document.getElementById(`span_${processedFileName}`);
    }

    private getDisplayOrderNumber(processedFileName: string): HTMLElement {
        return document.getElementById(`number_${processedFileName}`);
    }

    private getEditIcon(processedFileName: string): HTMLElement {
        return document.getElementById(`icon_${processedFileName}`);
    }

    constructor(settings: Settings, service: Service) {
        this.settings = settings;
        this.service = service;
    }

    public async updateUploadImageDisplayOrderAsync(albumName: string, processedFileName: string, value: any) {
        const result: ApiCallResult = await this.service.updateUploadImageDisplayOrderAsync(albumName, processedFileName, value);

        if (!result.ok) {
            alert(result.message);
            return;
        }

        this.getDisplayOrderSpan(processedFileName).innerText = result.data.displayOrder;
        this.toggleEditState(processedFileName, false);
    }

    public toggleEditState(processedFileName: string, isEditing: boolean) {
        const number = this.getDisplayOrderNumber(processedFileName);
        const span = this.getDisplayOrderSpan(processedFileName);
        const icon = this.getEditIcon(processedFileName);
        const none = "none";
        const inline = "inline";

        if (!isEditing) {
            number.style.display = none;
            span.style.display = inline;
            icon.style.display = inline;
        } else {
            number.style.display = inline;
            span.style.display = none;
            icon.style.display = none;
        }
    }
}