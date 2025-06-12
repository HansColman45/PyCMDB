import { type Locator, type Page } from '@playwright/test';

export class MainPage {
  readonly page: Page;
  readonly newButton: Locator;
  readonly editButton: Locator;
  readonly deleteButton: Locator;
  readonly detailsButton: Locator;

  constructor(page: Page) {
    this.page = page;
    this.newButton = page.getByRole('link', { name: 'Add' });
    this.editButton = page.getByRole('link', { name: 'Edit' });
    this.deleteButton = page.getByRole('link', { name: 'Deactivate' });
    this.detailsButton = page.getByRole('link', { name: 'Info' });
  }

  async goto() {
    await this.page.goto('https://localhost:44314/');
  }

  async IsLoggedIn(): Promise<boolean> {
    return await this.page.isVisible('text=Welcome');
  }
}