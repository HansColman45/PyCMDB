import { type Locator, type Page } from '@playwright/test';
import { IdentityOverviewPage } from './Identity/IdentityOverviewPage';
import { LoginPage } from './LoginPage';
export class MainPage extends LoginPage{ 
  readonly newButton: Locator;
  readonly editButton: Locator;
  readonly deleteButton: Locator;
  readonly detailsButton: Locator;
  readonly searchInput: Locator;

  constructor(page: Page) {
    super(page);
    this.newButton = page.getByRole('link', { name: 'Add' });
    this.editButton = page.getByRole('link', { name: 'Edit' });
    this.deleteButton = page.getByRole('link', { name: 'Deactivate' });
    this.detailsButton = page.getByRole('link', { name: 'Info' });
    this.searchInput = page.getByRole('textbox', { name: 'Search' });
  }

  async IsLoggedIn(): Promise<boolean> {
    return await this.page.getByText('Welcome on the Central').isVisible();
  }

  async IdentityOverviewPage(): Promise<IdentityOverviewPage> {
    await this.page.getByRole('button', { name: 'Identity' }).click();
    await this.page.locator('#Identity2').click();
    await this.page.getByRole('link', { name: 'Overview' }).click();
    await this.newButton.isVisible();
    return new IdentityOverviewPage(this.page);
  }

  async EnterTextInTextBox(xpath:string, text:string): Promise<void> {
    await this.page.locator('xpath='+xpath).fill(text);
  }

}