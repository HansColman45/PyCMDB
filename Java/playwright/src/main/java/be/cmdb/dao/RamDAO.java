package be.cmdb.dao;

import be.cmdb.model.RAM;
import org.hibernate.Session;
import org.hibernate.query.Query;

import java.util.List;

public class RamDAO implements BaseDAO<RAM, Integer> {
    @Override
    public RAM save(Session session, RAM entity) {
        session.persist(entity);
        return entity;
    }

    @Override
    public RAM update(Session session, RAM entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public RAM findById(Session session, Integer id) {
        return session.find(RAM.class, id);
    }

    @Override
    public List<RAM> findAll(Session session) {
        Query<RAM> query = session.createQuery("FROM RAM", RAM.class);
        return query.getResultList();
    }

    @Override
    public void delete(Session session, RAM entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, Integer id) {
        RAM ram = findById(session, id);
        if (ram != null) {
            delete(session, ram);
        }
    }

    @Override
    public long count(Session session) {
        Query<Long> query = session.createQuery("SELECT COUNT(r) FROM RAM r", Long.class);
        return query.uniqueResult();
    }
}
