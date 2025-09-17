package be.cmdb.dao;

import be.cmdb.model.IdenAccount;
import org.hibernate.Session;
import org.hibernate.query.Query;

import java.util.List;

/**
 * Data Access Object for IdenAccount entity operations.
 * Provides CRUD operations and specialized queries for IdenAccount entities.
 * All operations use the provided Hibernate Session for database access.
 * Transaction management should be handled externally.
 */
public class IdenAccountDAO implements BaseDAO<IdenAccount, Integer> {
    @Override
    public IdenAccount save(Session session, IdenAccount entity) {
        session.persist(entity);
        return entity;
    }

    @Override
    public IdenAccount update(Session session, IdenAccount entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public IdenAccount findById(Session session, Integer integer) {
        return session.find(IdenAccount.class, integer);
    }

    @Override
    public List<IdenAccount> findAll(Session session) {
        Query<IdenAccount> query = session.createQuery("FROM IdenAccount", IdenAccount.class);
        return query.getResultList();
    }

    @Override
    public void delete(Session session, IdenAccount entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, Integer integer) {
        IdenAccount idenAccount = findById(session, integer);
        if (idenAccount != null) {
            delete(session, idenAccount);
        }
    }

    @Override
    public long count(Session session) {
        Query<Long> query = session.createQuery("SELECT COUNT(ia) FROM IdenAccount ia", Long.class);
        return query.getSingleResult();
    }

    /**
     * Finds all IdenAccount entities where the lastModifiedBy admin has the specified adminId.
     * @param session the Hibernate session to use
     * @param adminId the ID of the admin who last modified the IdenAccount
     * @return list of IdenAccount entities modified by the specified admin
     */
    public List<IdenAccount> findByLastModifiedAdminId(Session session, Integer adminId) {
        Query<IdenAccount> query = session.createQuery(
            "FROM IdenAccount ia WHERE ia.lastModifiedAdminId = :adminId", IdenAccount.class);
        query.setParameter("adminId", adminId);
        return query.getResultList();
    }
}
