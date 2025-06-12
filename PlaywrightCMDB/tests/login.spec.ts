import { test} from '@playwright/test';
import { LoginPage } from '../src/Pages/LoginPage';

test("Can login", async ({ page }) => {
  const loginPage = new LoginPage(page);
  await loginPage.goto();
  await loginPage.login("Root", "796724Md");
  const isLoggedIn = await loginPage.IsLoggedIn();
  if (!isLoggedIn) {
    throw new Error("Login failed");
  }
});