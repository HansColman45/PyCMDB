package be.cmdb.dao;

import be.cmdb.model.Identity;
import java.util.List;
import org.hibernate.Session;
import org.hibernate.query.Query;

/**
 * Data Access Object for Identity entity operations.
 * Provides CRUD operations and specialized queries for Identity entities.
 * All operations use the provided Hibernate Session for database access.
 * Transaction management should be handled externally.
 */
public class IdentityDAO implements BaseDAO<Identity, Integer> {

    /**
     * Default constructor.
     */
    public IdentityDAO() {
    }

    /**
     * Saves a new identity to the database using the provided session.
     * @param session the Hibernate session to use
     * @param identity the Identity entity to save
     * @return the saved Identity entity with generated ID
     */
    @Override
    public Identity save(Session session, Identity identity) {
        session.persist(identity);
        return identity;
    }

    /**
     * Updates an existing identity in the database using the provided session.
     * @param session the Hibernate session to use
     * @param identity the Identity entity to update
     * @return the updated Identity entity
     */
    @Override
    public Identity update(Session session, Identity identity) {
        return session.merge(identity);
    }

    /**
     * Finds an identity by its ID using the provided session.
     * @param session the Hibernate session to use
     * @param id the ID of the identity
     * @return the Identity entity if found, null otherwise
     */
    @Override
    public Identity findById(Session session, Integer id) {
        return session.find(Identity.class, id);
    }

    /**
     * Retrieves all identities from the database using the provided session.
     * @param session the Hibernate session to use
     * @return list of all Identity entities
     */
    @Override
    public List<Identity> findAll(Session session) {
        Query<Identity> query = session.createQuery("FROM Identity", Identity.class);
        return query.getResultList();
    }

    /**
     * Deletes an identity from the database using the provided session.
     * @param session the Hibernate session to use
     * @param identity the Identity entity to delete
     */
    @Override
    public void delete(Session session, Identity identity) {
        session.remove(identity);
    }

    /**
     * Deletes an identity by its ID using the provided session.
     * @param session the Hibernate session to use
     * @param id the ID of the identity to delete
     */
    @Override
    public void deleteById(Session session, Integer id) {
        Identity identity = session.find(Identity.class, id);
        if (identity != null) {
            session.remove(identity);
        }
    }

    /**
     * Finds an identity by email address using the provided session.
     * @param session the Hibernate session to use
     * @param email the email address to search for
     * @return the Identity entity if found, null otherwise
     */
    public Identity findByEmail(Session session, String email) {
        Query<Identity> query = session.createQuery(
            "FROM Identity WHERE email = :email", Identity.class);
        query.setParameter("email", email);
        return query.uniqueResult();
    }

    /**
     * Finds an identity by user ID using the provided session.
     * @param session the Hibernate session to use
     * @param userId the user ID to search for
     * @return the Identity entity if found, null otherwise
     */
    public Identity findByUserId(Session session, String userId) {
        Query<Identity> query = session.createQuery(
            "FROM Identity WHERE userId = :userId", Identity.class);
        query.setParameter("userId", userId);
        return query.uniqueResult();
    }

    /**
     * Finds identities by company name using the provided session.
     * @param session the Hibernate session to use
     * @param company the company name to search for
     * @return list of Identity entities from the specified company
     */
    public List<Identity> findByCompany(Session session, String company) {
        Query<Identity> query = session.createQuery(
            "FROM Identity WHERE company = :company ORDER BY name", Identity.class);
        query.setParameter("company", company);
        return query.getResultList();
    }

    /**
     * Finds identities by type ID using the provided session.
     * @param session the Hibernate session to use
     * @param typeId the type ID to search for
     * @return list of Identity entities with the specified type
     */
    public List<Identity> findByTypeId(Session session, int typeId) {
        Query<Identity> query = session.createQuery(
            "FROM Identity WHERE typeId = :typeId ORDER BY name", Identity.class);
        query.setParameter("typeId", typeId);
        return query.getResultList();
    }

    /**
     * Finds identities by language code using the provided session.
     * @param session the Hibernate session to use
     * @param languageCode the language code to search for
     * @return list of Identity entities with the specified language
     */
    public List<Identity> findByLanguageCode(Session session, String languageCode) {
        Query<Identity> query = session.createQuery(
            "FROM Identity WHERE languageCode = :languageCode ORDER BY name", Identity.class);
        query.setParameter("languageCode", languageCode);
        return query.getResultList();
    }

    /**
     * Finds active identities using the provided session.
     * @param session the Hibernate session to use
     * @return list of active Identity entities
     */
    public List<Identity> findActiveIdentities(Session session) {
        Query<Identity> query = session.createQuery(
            "FROM Identity WHERE active = 1 ORDER BY name", Identity.class);
        return query.getResultList();
    }

    /**
     * Searches identities by name (partial match) using the provided session.
     * @param session the Hibernate session to use
     * @param namePattern the name pattern to search for
     * @return list of Identity entities matching the name pattern
     */
    public List<Identity> findByNameContaining(Session session, String namePattern) {
        Query<Identity> query = session.createQuery(
            "FROM Identity WHERE name LIKE :namePattern ORDER BY name", Identity.class);
        query.setParameter("namePattern", "%" + namePattern + "%");
        return query.getResultList();
    }

    /**
     * Searches identities by email domain using the provided session.
     * @param session the Hibernate session to use
     * @param domain the email domain to search for (e.g., "company.com")
     * @return list of Identity entities with emails from the specified domain
     */
    public List<Identity> findByEmailDomain(Session session, String domain) {
        Query<Identity> query = session.createQuery(
            "FROM Identity WHERE email LIKE :domain ORDER BY name", Identity.class);
        query.setParameter("domain", "%@" + domain);
        return query.getResultList();
    }

    /**
     * Finds identities with pagination support using the provided session.
     * @param session the Hibernate session to use
     * @param offset the number of records to skip
     * @param limit the maximum number of records to return
     * @return list of Identity entities for the specified page
     */
    public List<Identity> findWithPagination(Session session, int offset, int limit) {
        Query<Identity> query = session.createQuery(
            "FROM Identity ORDER BY name", Identity.class);
        query.setFirstResult(offset);
        query.setMaxResults(limit);
        return query.getResultList();
    }

    /**
     * Counts the total number of identities using the provided session.
     * @param session the Hibernate session to use
     * @return the total count of identities
     */
    @Override
    public long count(Session session) {
        Query<Long> query = session.createQuery("SELECT COUNT(*) FROM Identity", Long.class);
        return query.getSingleResult();
    }

    /**
     * Counts identities by company using the provided session.
     * @param session the Hibernate session to use
     * @param company the company name to count identities for
     * @return the count of identities for the specified company
     */
    public long countByCompany(Session session, String company) {
        Query<Long> query = session.createQuery(
                "SELECT COUNT(*) FROM Identity WHERE company = :company", Long.class);
        query.setParameter("company", company);
        return query.getSingleResult();
    }

    /**
     * Counts identities by type ID using the provided session.
     * @param session the Hibernate session to use
     * @param typeId the type ID to count identities for
     * @return the count of identities for the specified type
     */
    public long countByTypeId(Session session, int typeId) {
        Query<Long> query = session.createQuery(
            "SELECT COUNT(*) FROM Identity WHERE typeId = :typeId", Long.class);
        query.setParameter("typeId", typeId);
        return query.getSingleResult();
    }

    /**
     * Checks if an email address already exists in the database using the provided session.
     * @param session the Hibernate session to use
     * @param email the email address to check
     * @return true if the email exists, false otherwise
     */
    public boolean emailExists(Session session, String email) {
        Query<Long> query = session.createQuery(
            "SELECT COUNT(*) FROM Identity WHERE email = :email", Long.class);
        query.setParameter("email", email);
        return query.getSingleResult() > 0;
    }

    /**
     * Checks if a user ID already exists in the database using the provided session.
     * @param session the Hibernate session to use
     * @param userId the user ID to check
     * @return true if the user ID exists, false otherwise
     */
    public boolean userIdExists(Session session, String userId) {
        Query<Long> query = session.createQuery(
            "SELECT COUNT(*) FROM Identity WHERE userId = :userId", Long.class);
        query.setParameter("userId", userId);
        return query.getSingleResult() > 0;
    }
}
