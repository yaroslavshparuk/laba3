import { User } from "./user";

export class HistoryDetail{
    workItemNum:number;
    newRemainHours: number;
    oldRemainHours:number;
    revisionBy: User;
    revisionTime: Date;
}