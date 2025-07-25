package be.cmdb.dao;

import be.cmdb.model.Log;
import java.util.List;
import org.hibernate.JDBCException;
import org.hibernate.Session;
import org.hibernate.query.Query;

/**
* Data Access Object for Log entity operations.
* Provides CRUD operations and specialized queries for Log entities.
* All operations use the provided Hibernate Session for database access.
* Transaction management should be handled externally.
*/
public class LogDAO implements BaseDAO<Log, Integer> {

    /**
     * Default constructor.
     */
    public LogDAO() {
    }

    /**
     * Saves a new log entry to the database using the provided session.
     * @param session the Hibernate session to use
     * @param log the Log entity to save
     * @return the saved Log entity with generated ID
     */
    @Override
    public Log save(Session session, Log log) {
        try {
            session.persist(log);
            return log;
        } catch (JDBCException e) {
            throw new RuntimeException("Error saving log: " + e.getMessage(), e);
        }
    }

    /**
     * Updates an existing log entry in the database using the provided session.
     * @param session the Hibernate session to use
     * @param log the Log entity to update
     * @return the updated Log entity
     */
    @Override
    public Log update(Session session, Log log) {
        try {
            return session.merge(log);
        } catch (JDBCException e) {
            throw new RuntimeException("Error updating log: " + e.getMessage(), e);
        }
    }

    /**
     * Finds a log entry by its ID using the provided session.
     * @param session the Hibernate session to use
     * @param id the ID of the log entry
     * @return the Log entity if found, null otherwise
     */
    @Override
    public Log findById(Session session, Integer id) {
        try {
            return session.find(Log.class, id);
        } catch (JDBCException e) {
            throw new RuntimeException("Error finding log by ID: " + e.getMessage(), e);
        }
    }

    /**
     * Retrieves all log entries from the database using the provided session.
     * @param session the Hibernate session to use
     * @return list of all Log entities
     */
    @Override
    public List<Log> findAll(Session session) {
        try {
            Query<Log> query = session.createQuery("FROM Log", Log.class);
            return query.getResultList();
        } catch (JDBCException e) {
            throw new RuntimeException("Error retrieving all logs: " + e.getMessage(), e);
        }
    }

    /**
     * Deletes a log entry from the database using the provided session.
     * @param session the Hibernate session to use
     * @param log the Log entity to delete
     */
    @Override
    public void delete(Session session, Log log) {
        try {
            session.remove(log);
        } catch (JDBCException e) {
            throw new RuntimeException("Error deleting log: " + e.getMessage(), e);
        }
    }

    /**
     * Deletes a log entry by its ID using the provided session.
     * @param session the Hibernate session to use
     * @param id the ID of the log entry to delete
     */
    @Override
    public void deleteById(Session session, Integer id) {
        try {
            Log log = session.find(Log.class, id);
            if (log != null) {
                session.remove(log);
            }
        } catch (JDBCException e) {
            throw new RuntimeException("Error deleting log by ID: " + e.getMessage(), e);
        }
    }

    /**
     * Finds log entries by identity ID using the provided session.
     * @param session the Hibernate session to use
     * @param identityId the identity ID to search for
     * @return list of Log entities associated with the identity
     */
    public List<Log> findByIdentityId(Session session, int identityId) {
        try {
            Query<Log> query = session.createQuery(
                "FROM Log WHERE identityId = :identityId ORDER BY logDate DESC", Log.class);
            query.setParameter("identityId", identityId);
            return query.getResultList();
        } catch (JDBCException e) {
            throw new RuntimeException("Error finding logs by identity ID: " + e.getMessage(), e);
        }
    }

    /**
     * Finds log entries by account ID using the provided session.
     * @param session the Hibernate session to use
     * @param accountId the account ID to search for
     * @return list of Log entities associated with the account
     */
    public List<Log> findByAccountId(Session session, int accountId) {
        try {
            Query<Log> query = session.createQuery(
                "FROM Log WHERE accountId = :accountId ORDER BY logDate DESC", Log.class);
            query.setParameter("accountId", accountId);
            return query.getResultList();
        } catch (JDBCException e) {
            throw new RuntimeException("Error finding logs by account ID: " + e.getMessage(), e);
        }
    }

    /**
     * Counts the total number of log entries using the provided session.
     * @param session the Hibernate session to use
     * @return the total count of log entries
     */
    @Override
    public long count(Session session) {
        try {
            Query<Long> query = session.createQuery("SELECT COUNT(*) FROM Log", Long.class);
            return query.getSingleResult();
        } catch (JDBCException e) {
            throw new RuntimeException("Error counting logs: " + e.getMessage(), e);
        }
    }

    /**
     * Finds log entries by key ID using the provided session.
     * @param session the Hibernate session to use
     * @param keyId the key ID to search for
     * @return list of Log entities associated with the key
     */
    public List<Log> findByKeyId(Session session, int keyId) {
        Query<Log> query = session.createQuery(
            "FROM Log WHERE kensingtonId = :keyId", Log.class);
        query.setParameter("keyId", keyId);
        return query.getResultList();
    }

    /**
     * Finds log entries by mobile ID using the provided session.
     * @param session the Hibernate session to use
     * @param mobileId the mobile ID to search for
     * @return list of Log entities associated with the mobile
     */
    public List<Log> findByMobileId(Session session, int mobileId) {
        Query<Log> query = session.createQuery(
            "FROM Log WHERE mobileId = :mobileId", Log.class);
        query.setParameter("mobileId", mobileId);
        return query.getResultList();
    }

    /**
     * Finds log entries by type ID using the provided session.
     * @param session the Hibernate session to use
     * @param typeId the type ID to search for
     * @return list of Log entities associated with the type
     */
    public List<Log> findByTypeId(Session session, int typeId) {
        Query<Log> query = session.createQuery(
            "FROM Log WHERE typeId = :typeId", Log.class);
        query.setParameter("typeId", typeId);
        return query.getResultList();
    }

    /**
     * Finds log entries by asset type ID using the provided session.
     * @param session the Hibernate session to use
     * @param typeId the asset type ID to search for
     * @return list of Log entities associated with the asset type
     */
    public List<Log> findbyAssetTypeId(Session session, int typeId) {
        Query<Log> query = session.createQuery(
            "FROM Log WHERE assetTypeId = :typeId", Log.class);
        query.setParameter("typeId", typeId);
        return query.getResultList();
    }
}
