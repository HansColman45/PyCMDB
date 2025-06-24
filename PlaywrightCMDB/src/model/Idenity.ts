export class Identity {
    lastName:string;
    idenId:string;
    firstName:string;
    email:string;
    company:string;
    userId:string;

    getUserId() {
        return this.userId;
    }
    setUserId(value) {
        this.userId = value;
    }

    getLastName() {
        return this.lastName;
    }
    setLastName(value: string) {
        this.lastName = value;
    }

    getIdenId() {
        return this.idenId;
    }
    setIdenId(value:string) {
        this.idenId = value;
    }

    getFirstName() {
        return this.firstName;
    }
    setFirstName(value:string) {
        this.firstName = value;
    }

    getEmail() {
        return this.email;
    }
    setEmail(value:string) {
        this.email = value;
    }

    getCompany() {
        return this.company;
    }
    setCompany(value:string) {
        this.company = value;
    }
}