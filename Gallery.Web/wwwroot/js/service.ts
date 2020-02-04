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
        //.replace('.jpg', '').replace('.png', '');
        //TODO: Replace with regex.
        const uri = `/api/album/${albumName}/image/${processedFileName}`;
        const rawResponse = await fetch(uri, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        });

        return !!rawResponse.ok
            ? new ApiCallResult().withSuccess('OK')
            : new ApiCallResult().withError(rawResponse.statusText, rawResponse.status);
    }
}
