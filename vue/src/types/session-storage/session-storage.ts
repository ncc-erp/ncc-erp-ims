export default interface IInfoPage {
    entity;
    title;
    type;
    status;
    pageNumber;
}

export default class SessionStorageInfoPage {
    public static save(infoPage: IInfoPage){
        let entityPage = {
            entity: infoPage.entity,
            title: infoPage.title,
            type: infoPage.type,
            status: infoPage.status,
            pageNumber: infoPage.pageNumber
        }

        sessionStorage.setItem(infoPage.entity + 'InfoPage', JSON.stringify(entityPage));
    }

    public static getItem(entity){
        return JSON.parse(sessionStorage.getItem(entity + 'InfoPage'));
    }

    public static clear(entity){
        sessionStorage.removeItem(entity + 'InfoPage');
    }

    public static hasValue(entity):boolean{        
        return sessionStorage.getItem(entity + 'InfoPage') != null;
    }
}