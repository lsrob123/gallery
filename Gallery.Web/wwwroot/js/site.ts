class Album {
    private readonly settings: Settings;
    private readonly service: Service;

    constructor(settings: Settings, service:Service) {
        this.settings = settings;
        this.service = service;
    }

    public updateUploadImageDisplayOrderAsync = async (albumName: string, processedFileName: string, value: any) => {
        const result = await this.service.updateUploadImageDisplayOrderAsync(albumName, processedFileName, value);
        alert(result.message);
    }
}

const settings = new Settings();
const service = new Service(settings);
const album = new Album(settings, service);