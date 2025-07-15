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
}
