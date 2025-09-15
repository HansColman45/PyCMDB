package be.cmdb.helpers;

import be.cmdb.dao.KensingtonDAO;
import be.cmdb.dao.LogDAO;
import be.cmdb.model.Kensington;
import be.cmdb.model.Log;
import org.hibernate.Session;
import org.hibernate.Transaction;

import java.util.List;

public class KeyHelper {

    /**
     * Deletes a Kensington key and all associated logs from the database.
     * @param session the Hibernate session
     * @param kensington the Kensington entity to be deleted
     */
    public static void deleteKey(Session session, Kensington kensington){
        Transaction transaction = session.beginTransaction();
        KensingtonDAO kensingtonDAO = new KensingtonDAO();
        LogDAO logDAO = new LogDAO();
        List<Log> logs = logDAO.findByKensingtonId(session, kensington.getKeyId());
        for (Log log : logs) {
            logDAO.delete(session, log);
        }
        kensingtonDAO.delete(session, kensington);
        transaction.commit();
    }
}
