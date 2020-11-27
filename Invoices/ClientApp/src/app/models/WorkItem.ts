import { HistoryDetail } from "./HistoryDetail";

export class WorkItem{
    externalId:number;
    name: string;
    createdDate:Date;
    lastUpdatedDate:Date;
    closedDate:Date;
    title:string;
    type:string;
    status:string;
    History: HistoryDetail[]
}