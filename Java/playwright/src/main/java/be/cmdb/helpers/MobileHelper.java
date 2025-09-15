package be.cmdb.helpers;

import be.cmdb.dao.LogDAO;
import be.cmdb.dao.MobileDAO;
import be.cmdb.model.Log;
import be.cmdb.model.Mobile;
import org.hibernate.Session;
import org.hibernate.Transaction;

import java.util.List;

public class MobileHelper {

    /**
     * Deletes a Mobile device and all associated logs from the database.
     * @param session the Hibernate session
     * @param mobile the Mobile entity to be deleted
     */
    public static void deleteMobile(Session session, Mobile mobile) {
        Transaction transaction = session.beginTransaction();
        MobileDAO mobileDAO = new MobileDAO();
        LogDAO logDAO = new LogDAO();
        List<Log> logs = logDAO.findByMobileId(session, mobile.getMobileId());
        for (Log log : logs) {
            logDAO.delete(session, log);
        }
        mobileDAO.delete(session, mobile);
        transaction.commit();
    }
}
