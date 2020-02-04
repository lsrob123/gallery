class Service {
    private readonly settings: Settings;

    constructor(settings: Settings) {
        this.settings = settings;
    }

    public updateUploadImageDisplayOrderAsync = async (albumName: string, processedFileName: string, value: any): Promise<ApiCallResult> => {
        const request = {
            displayOrder: parseInt(value)
        };
        processedFileName = processedFileName.toLowerCase();

        const uri = `/api/album/${albumName}/image/${processedFileName}`;
        const rawResponse = await fetch(uri, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        if (!!rawResponse.ok) {
            const data = await rawResponse.json();
            return new ApiCallResult().withSuccess(data);
        }

        return new ApiCallResult().withError(rawResponse.statusText, rawResponse.status);
    }
}
