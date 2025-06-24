import { Page, Locator } from '@playwright/test';
import { LoginPage } from '../LoginPage';
export class IdentityOverviewPage extends LoginPage {
    readonly newButton: Locator;
    readonly searchInput: Locator;

    constructor(page:Page) {
        super(page);
        this.newButton = page.getByRole('link', { name: 'Add' });
        this.searchInput = page.getByRole('textbox', { name: 'Search' });
    }

    async Search(searchText: string){
        await this.searchInput.fill(searchText);
        await this.searchInput.press('Enter');
    }
}