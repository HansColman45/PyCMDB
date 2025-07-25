package be.cmdb.dao;

import be.cmdb.model.Key;
import org.hibernate.Session;
import org.hibernate.query.Query;
import java.util.List;

public class KeyDAO implements BaseDAO<Key, Integer> {

    @Override
    public Key save(Session session, Key entity) {
        session.persist(entity);
        return entity;
    }

    @Override
    public Key update(Session session, Key entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public Key findById(Session session, Integer id) {
        return session.find(Key.class, id);
    }

    @Override
    public List<Key> findAll(Session session) {
        Query<Key> query = session.createQuery("FROM Key", Key.class);
        return query.getResultList();
    }

    @Override
    public void delete(Session session, Key entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, Integer id) {
        Key key = findById(session, id);
        if (key != null) {
            delete(session, key);
        } else {
            // Optionally handle the case where the key is not found
            System.out.println("Key with ID " + id + " not found.");
        }
    }

    @Override
    public long count(Session session) {
        Query<Long> query = session.createQuery("SELECT COUNT(k) FROM Key k", Long.class);
        return query.getSingleResult();
    }

    public List<Key> findByLastModifiedAdminId(Session session, Integer adminId) {
        Query<Key> query = session.createQuery("FROM Key k WHERE k.lastModifiedAdminId = :adminId", Key.class);
        query.setParameter("adminId", adminId);
        return query.getResultList();
    }
}
