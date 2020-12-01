import { HistoryDetail } from "./HistoryDetail";

export class WorkItem {
    constructor(
        public externalId?:number,
        public name?: string,
        public createdDate?:Date,
        public lastUpdatedDate?:Date,
        public closedDate?:Date,
        public title?:string,
        public type?:string,
        public status?:string,
        public History?: HistoryDetail[]) { }
}