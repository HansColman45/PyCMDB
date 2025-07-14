package be.cmdb.helpers;

import be.cmdb.dao.AccountTypeDAO;
import be.cmdb.dao.IdentityTypeDAO;
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
    * returns the created Admin object.
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
}
