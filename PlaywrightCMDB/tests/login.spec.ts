import { test} from '@playwright/test';
import { LoginPage, MainPage } from '../src/Pages';

test("Can login", async ({ page }) => {
  const loginPage = new LoginPage(page);
  await loginPage.goto();
  await loginPage.login("Root", "796724Md");
  let mainPage = new MainPage(page);  
  const isLoggedIn = await mainPage.IsLoggedIn();
  if (!isLoggedIn) {
    throw new Error("Login failed");
  }
});