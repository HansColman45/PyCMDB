import { Identity } from "../model/Idenity";
import { fakerNL } from '@faker-js/faker';

export class IdentityHelper {
    static createIdentity(): Identity {
        const iden:Identity = new Identity();
        iden.firstName = fakerNL.person.firstName();
        iden.lastName = fakerNL.person.lastName();
        iden.userId = fakerNL.internet.username();
        iden.email = fakerNL.internet.email();
        iden.company = fakerNL.company.name();
        return iden;
    }
}