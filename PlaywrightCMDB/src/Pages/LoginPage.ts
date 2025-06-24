import { type Locator, type Page } from '@playwright/test';

export class LoginPage {
    readonly usernameInput: Locator;
    readonly passwordInput: Locator;
    readonly loginButton: Locator;
    readonly page: Page;

    constructor(page: Page) {
        this.page = page;
        this.usernameInput = page.locator('xpath=//input[@type=\'text\']');
        this.passwordInput = page.locator('xpath=//input[@type=\'password\']');
        this.loginButton = page.getByRole('button');
    }

    async goto() {
        await this.page.goto('http://localhost:44313/');
    }
        

    async login(username: string, password: string) {
        await this.usernameInput.fill(username);
        await this.passwordInput.fill(password);
        await this.loginButton.click();
    }
}