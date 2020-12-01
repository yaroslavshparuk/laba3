import { User } from "./user";

export class HistoryDetail {
    constructor(
        public workItemNum?:number,
        public newRemainHours?: number,
        public oldRemainHours?:number,
        public revisionBy?: User,
        public revisionTime?: Date) { }
}