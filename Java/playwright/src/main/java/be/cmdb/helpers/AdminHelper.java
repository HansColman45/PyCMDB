package be.cmdb.helpers;

import be.cmdb.dao.AccountTypeDAO;
import be.cmdb.dao.IdentityTypeDAO;
import be.cmdb.model.Account;
import be.cmdb.model.Admin;
import be.cmdb.model.GeneralType;
import be.cmdb.model.Identity;
import org.hibernate.Session;
import org.hibernate.Transaction;

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

        // First create an Identity
        IdentityTypeDAO identityTypeDAO = new IdentityTypeDAO();
        GeneralType identityType = identityTypeDAO.findByType(session, "Werknemer");

        AccountTypeDAO accountTypeDAO = new AccountTypeDAO();
        GeneralType accountType = accountTypeDAO.findByType(session, "Administrator");

        Identity identity = IdentityHelper.createRandomIdentity(session, identityType.getTypeId());

        Admin admin = new Admin();
        return admin;
    }

    public static Admin CreateSimpleAdmin(Session session, Account account, Admin admin, int level, boolean active) {
        Admin = new Ad
    }
}
