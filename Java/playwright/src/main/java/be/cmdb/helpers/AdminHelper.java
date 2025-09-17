package be.cmdb.helpers;

import java.time.LocalDateTime;
import java.time.ZoneOffset;
import java.util.List;

import be.cmdb.dao.*;
import be.cmdb.model.*;
import org.hibernate.Session;
import org.hibernate.Transaction;

import be.cmdb.builders.AccountBuilder;
import be.cmdb.builders.AdminBuilder;
import be.cmdb.builders.IdentityBuilder;
import be.cmdb.builders.LogBuilder;

/**
 * Helper class for Admin operations in the CMDB application.
 * Provides methods to create and manage Admin entities.
 */
public final class AdminHelper {

    private AdminHelper() {
    }

    /**
    * Creates a new Admin that will be used for the test.
    * @param session the Hibernate session
    * @return a new Admin entity persisted in the database
    */
    public static Admin createNewCMDBAdmin(Session session) {
        Transaction transaction = session.beginTransaction();

        // Get the needed types from the database
        IdentityTypeDAO identityTypeDAO = new IdentityTypeDAO();
        Type identityType = identityTypeDAO.findByType(session, "Werknemer");
        AccountTypeDAO accountTypeDAO = new AccountTypeDAO();
        Type accountType = accountTypeDAO.findByType(session, "Administrator");
        ApplicationDAO applicationDAO = new ApplicationDAO();
        Application application = applicationDAO.findByName(session, "CMDB").get(0);

        //First create the account
        Account account = new AccountBuilder()
            .withLastModifiedAdminId(1)
            .withTypeId(accountType)
            .withApplicationId(application.getAppId())
            .build();
        AccountDAO accountDAO = new AccountDAO();
        account = accountDAO.save(session, account);

        Log accountLog = new LogBuilder()
            .withAccount(account)
            .withLogText("The Account with UserID: " + account.getUserId()
                +" for application " + application.getName() +
                " is created by Automation in table account")
            .build();
        LogDAO logDAO = new LogDAO();
        logDAO.save(session, accountLog);

        // Now create the admin
        Admin admin = new AdminBuilder()
            .withLastModifiedAdminId(1)
            .withAccountId(account.getAccId())
            .build();
        AdminDAO adminDAO = new AdminDAO();
        admin = adminDAO.save(session, admin);

        Log adminLog = new LogBuilder()
            .withAdmin(admin)
            .withLogText("The Admin with UserId: " + account.getUserId()
                + " is created by Automation in table admin")
            .build();
        logDAO.save(session, adminLog);

        //Create the identity for the account
        Identity identity = new IdentityBuilder()
            .withTypeId(identityType)
            .withUserId(account.getUserId())
            .withLastModifiedAdminId(admin.getAdminId())
            .build();
        IdentityDAO identityDAO = new IdentityDAO();
        identity = identityDAO.save(session, identity);

        Log identityLog = new LogBuilder()
            .withIdentity(identity)
            .withLogText("The Identity with UserID: " + account.getUserId()
                + " is created by Automation in table identity")
            .build();
        logDAO.save(session, identityLog);

        IdenAccount idenAccount = new IdenAccount();
        idenAccount.setAccountId(account.getAccId());
        idenAccount.setIdentityId(identity.getIdenId());
        idenAccount.setLastModifiedAdminId(admin.getAdminId());
        idenAccount.setValidFrom(LocalDateTime.now(ZoneOffset.UTC));
        idenAccount.setValidUntil(LocalDateTime.now(ZoneOffset.UTC).plusYears(5));

        IdenAccountDAO dao = new IdenAccountDAO();
        dao.save(session, idenAccount);

        transaction.commit();
        return admin;
    }

    /**
     * Creates a simple Admin with the specified level and active status.
     * @param session the Hibernate session
     * @param account the Account associated with the Admin
     * @param admin the Admin who is creating this new Admin
     * @param level the level of the new Admin
     * @param active whether the new Admin is active
     * @return a new Admin entity persisted in the database
     */
    public static Admin createSimpleAdmin(Session session, Account account, Admin admin, int level, boolean active) {
        Transaction transaction = session.beginTransaction();
        Admin newAdmin = new AdminBuilder()
            .withAccountId(account.getAccId())
            .withLastModifiedAdminId(admin.getLastModifiedAdminId())
            .withLevel(level)
            .withActive(active ? 1 : 0)
            .build();
        AdminDAO adminDAO = new AdminDAO();
        newAdmin = adminDAO.save(session, newAdmin);

        Log log = new LogBuilder()
            .withAdmin(newAdmin)
            .withLogText("The Admin with AccountID: " + account.getAccId()
                + " and level " + level + " is created by Automation in table admin")
            .build();

        LogDAO logDAO = new LogDAO();
        logDAO.save(session, log);
        transaction.commit();
        return newAdmin;
    }

    /**
     * Deletes all objects created by the specified Admin, including associated logs and the Admin itself.
     * @param session the Hibernate session
     * @param admin the Admin whose created objects are to be deleted
     */
    public static void deleteAllObjectsCreatedByAdmin(Session session, Admin admin) {
        //Keys
        KensingtonDAO kensingtonDAO = new KensingtonDAO();
        List<Kensington> kensingtons = kensingtonDAO.findByLastModifiedAdminId(session, admin.getAdminId());
        for (Kensington kensington : kensingtons) {
            KeyHelper.deleteKey(session, kensington);
        }
        //Mobiles
        MobileDAO mobileDAO = new MobileDAO();
        List<Mobile> mobiles = mobileDAO.findByLastModifiedAdminId(session, admin.getAdminId());
        for (Mobile mobile : mobiles) {
            MobileHelper.deleteMobile(session, mobile);
        }
        //AccountTypes
        AccountTypeDAO accountTypeDAO = new AccountTypeDAO();
        List<Type> accountTypes = accountTypeDAO.findByLastModifiedAdminId(session, admin.getAdminId());
        for (Type accountType : accountTypes) {
            AccountTypeHelper.deleteAccountType(session, accountType);
        }
        //AssetTypes
        AssetTypeDAO assetTypeDAO = new AssetTypeDAO();
        List<AssetType> assetTypes = assetTypeDAO.findByLastModifiedAdminId(session, admin.getAdminId());
        for (AssetType assetType : assetTypes) {
            AssetTypeHelper.deleteAssetType(session,assetType);
        }
        //IdenAccounts
        Transaction transaction = session.beginTransaction();
        IdenAccountDAO idenAccountDAO = new IdenAccountDAO();
        List<IdenAccount> idenAccounts = idenAccountDAO.findByLastModifiedAdminId(session,admin.getAdminId());
        for (IdenAccount idenAccount : idenAccounts) {
            idenAccountDAO.delete(session, idenAccount);
        }
        transaction.commit();
        //IdentityTypes
        IdentityTypeDAO identityTypeDAO = new IdentityTypeDAO();
        List<Type> identityTypes = identityTypeDAO.findByLastModifiedAdminId(session, admin.getAdminId());
        for (Type identityType : identityTypes) {
            IdentityTypeHelper.deleteType(session, identityType);
        }
        //Delete all Laptop created by this admin
        LaptopDAO laptopDAO = new LaptopDAO();
        List<Device> laptops = laptopDAO.findByLastModifiedAdminId(session, admin.getAdminId());
        for (Device laptop : laptops) {
            LaptopHelper.deleteLaptop(session, laptop);
        }
        //Delete all Identities created by this admin
        IdentityDAO identityDAO = new IdentityDAO();
        List<Identity> idens = identityDAO.findByLastModifiedAdminId(session, admin.getAdminId());
        for( Identity identity : idens) {
            IdentityHelper.deleteIdentity(session, identity);
        }

        transaction = session.beginTransaction();
        List<Log> logs = new LogDAO().findByAdminId(session, admin.getAdminId());
        for( Log log : logs) {
            //Delete the log
            new LogDAO().delete(session, log);
        }
        //Delete the Admin itself
        AdminDAO adminDAO = new AdminDAO();
        adminDAO.delete(session, admin);
        transaction.commit();
        //Delete the Account associated with the Admin
        AccountDAO accountDAO = new AccountDAO();
        Account account = accountDAO.findById(session, admin.getAccountId());
        AccountHelper.delete(session, account);
    }
}
