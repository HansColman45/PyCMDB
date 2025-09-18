package be.cmdb.dao;

import java.util.List;
import org.hibernate.Session;

/**
 * Generic DAO interface defining common CRUD operations.
 * All operations use the provided Hibernate Session for database access.
 * Transaction management should be handled externally.
 * @param <T> the entity type
 * @param <ID> the ID type
 */
public interface BaseDAO<T, ID> {

    /**
     * Saves a new entity to the database using the provided session.
     * @param session the Hibernate session to use
     * @param entity the entity to save
     * @return the saved entity with generated ID
     */
    T save(Session session, T entity);

    /**
     * Updates an existing entity in the database using the provided session.
     * @param session the Hibernate session to use
     * @param entity the entity to update
     * @return the updated entity
     */
    T update(Session session, T entity);

    /**
     * Finds an entity by its ID using the provided session.
     * @param session the Hibernate session to use
     * @param id the ID of the entity
     * @return the entity if found, null otherwise
     */
    T findById(Session session, ID id);

    /**
     * Retrieves all entities from the database using the provided session.
     * @param session the Hibernate session to use
     * @return list of all entities
     */
    List<T> findAll(Session session);

    /**
     * Deletes an entity from the database using the provided session.
     * @param session the Hibernate session to use
     * @param entity the entity to delete
     */
    void delete(Session session, T entity);

    /**
     * Deletes an entity by its ID using the provided session.
     * @param session the Hibernate session to use
     * @param id the ID of the entity to delete
     */
    void deleteById(Session session, ID id);

    /**
     * Counts the total number of entities using the provided session.
     * @param session the Hibernate session to use
     * @return the total count of entities
     */
    long count(Session session);
}
