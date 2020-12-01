import { User } from "./user";
import { WorkItem } from "./WorkItem";

export class UserWork {
    constructor(
        public user?: User,
        public workItem?:WorkItem,
        public date?:Date,
        public duration?: number) { }
}