import { User } from "../_models/user";

export class UserParams {               // instantiates a new instance of a user class, and we provide those parameters here
    gender: string;
    minAge = 18;
    maxAge = 99;                               // these defaults will be displayed to the user when they attempt to login
    pageNumber = 1;
    pageSize = 5;
    orderBy = 'lastActive';

    constructor(user: User) {
        this.gender = user.gender === 'female' ? 'male' : 'female';
    }
}