import { test} from '@playwright/test';
import { LoginPage, MainPage } from '../src/Pages';
import { IdentityHelper } from '../src/helpers/IdentityHelper.ts';
import { CreateIdentityPage } from '../src/Pages/Identity/CreateIdenityPage.ts';

test("Can create new identity", async ({ page }) => {
    const loginPage = new LoginPage(page);
    await loginPage.goto();
    await loginPage.login("Root", "796724Md");
    let mainPage = new MainPage(page);
    const isLoggedIn = await mainPage.IsLoggedIn();
    if (!isLoggedIn) {
        throw new Error("Login failed");
    }
    
    const identityOverviewPage = await mainPage.IdentityOverviewPage();
    await identityOverviewPage.newButton.click();
    
    const createIdentityPage = new CreateIdentityPage(page);
    await createIdentityPage.createButton.isVisible();

    let identity = IdentityHelper.createIdentity();
    await createIdentityPage.FirstName(identity.firstName);
    await createIdentityPage.LastName(identity.lastName);
    await createIdentityPage.UserId(identity.userId);
    await createIdentityPage.Email(identity.email);
    await createIdentityPage.Company(identity.company);
    await createIdentityPage.selectType("4");
    await createIdentityPage.selectLanguage("English");
    await createIdentityPage.create();
    await identityOverviewPage.newButton.isVisible();
    // Verify that the identity was created successfully
    await identityOverviewPage.Search(identity.userId);

});