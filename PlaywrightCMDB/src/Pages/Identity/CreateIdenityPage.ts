import { MainPage } from "../MainPage";
import { type Locator, type Page } from '@playwright/test';

export class CreateIdentityPage extends MainPage {
    readonly firstName: Locator;
    readonly lastName: Locator;
    readonly userId: Locator;
    readonly email: Locator;
    readonly company: Locator;
    readonly createButton: Locator;
    readonly languageSelector: Locator;
    readonly TypeSelector: Locator;

    constructor(page: Page) {
        super(page);
        this.firstName = page.locator("xpath=//input[@id='FirstName']");
        this.lastName = page.locator("xpath=//input[@id='LastName']");
        this.userId = page.locator("xpath=//input[@id='UserID']");
        this.email = page.locator("xpath=//input[@id='EMail']");
        this.company = page.locator("xpath=//input[@id='Company']");
        this.createButton = page.getByRole('button', { name: 'Create' });
        this.languageSelector = page.locator('#Language');
        this.TypeSelector = page.locator('#Type');
    }

    async FirstName(firstName: string): Promise<void> {
        await this.firstName.fill(firstName);
    }
    async LastName(lastName: string): Promise<void> {
        await this.lastName.fill(lastName);
    }
    async UserId(userId: string): Promise<void> {
        await this.userId.fill(userId);
    }
    async Email(email: string): Promise<void> {
        await this.email.fill(email);
    }
    async Company(company: string) {
        await this.company.fill(company);
    }
    async selectLanguage(language: string) {
        await this.languageSelector.selectOption({ label: language });
    }
    async selectType(type: string) {
        await this.TypeSelector.selectOption({ value: type });
    }
    async create() {
        await this.createButton.click();
    }
}