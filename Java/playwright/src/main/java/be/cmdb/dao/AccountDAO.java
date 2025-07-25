package be.cmdb.dao;

import be.cmdb.model.Account;
import org.hibernate.Session;
import org.hibernate.query.Query;
import java.util.List;

/**
 * Data Access Object for Account entities.
 */
public class AccountDAO implements BaseDAO<Account,Integer> {

    @Override
    public Account save(Session session, Account entity) {
        session.persist(entity);
        return entity;
    }

    @Override
    public Account update(Session session, Account entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public Account findById(Session session, Integer id) {
        return session.find(Account.class,id);
    }

    @Override
    public List<Account> findAll(Session session) {
        Query<Account> query = session.createQuery("FROM Account", Account.class);
        return query.getResultList();
    }

    @Override
    public void delete(Session session, Account entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, Integer integer) {
        Account account = findById(session, integer);
        if (account != null) {
            delete(session, account);
        } else {
            throw new IllegalArgumentException("Account with id " + integer + " does not exist.");
        }
    }

    @Override
    public long count(Session session) {
        Query<Long> query = session.createQuery("SELECT COUNT(*) FROM Account", Long.class);
        return query.getSingleResult();
    }

    /**
     * Finds accounts by the last modified admin ID.
     * @param session the Hibernate session
     * @param lastModifiedAdminId the ID of the admin who last modified the account
     * @return a list of accounts modified by the specified admin
     */
    public List<Account> findByLastModifiedAdminId(Session session, int lastModifiedAdminId) {
        Query<Account> query = session.createQuery("FROM Account WHERE lastModifiedAdminId = :lastModifiedAdminId", Account.class);
        query.setParameter("lastModifiedAdminId", lastModifiedAdminId);
        return query.getResultList();
    }
}
