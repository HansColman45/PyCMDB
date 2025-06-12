import { MainPage } from "./MainPage";
import { type Locator, type Page } from '@playwright/test';

export class LoginPage extends MainPage {
    readonly usernameInput: Locator;
    readonly passwordInput: Locator;
    readonly loginButton: Locator;
    constructor(page: Page) {
        super(page);
        this.usernameInput = page.locator('xpath=//input[@type=\'text\']');
        this.passwordInput = page.locator('xpath=//input[@type=\'password\']');
        this.loginButton = page.getByRole('button');
    }


    async login(username: string, password: string) {
        await this.usernameInput.fill(username);
        await this.passwordInput.fill(password);
        await this.loginButton.click();
    }
}