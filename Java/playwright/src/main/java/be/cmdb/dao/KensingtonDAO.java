package be.cmdb.dao;

import be.cmdb.model.Kensington;
import org.hibernate.Session;
import org.hibernate.query.Query;
import java.util.List;

public class KensingtonDAO implements BaseDAO<Kensington, Integer> {

    @Override
    public Kensington save(Session session, Kensington entity) {
        session.persist(entity);
        return entity;
    }

    @Override
    public Kensington update(Session session, Kensington entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public Kensington findById(Session session, Integer id) {
        return session.find(Kensington.class, id);
    }

    @Override
    public List<Kensington> findAll(Session session) {
        Query<Kensington> query = session.createQuery("FROM Kensington", Kensington.class);
        return query.getResultList();
    }

    @Override
    public void delete(Session session, Kensington entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, Integer id) {
        Kensington kensington = findById(session, id);
        if (kensington != null) {
            delete(session, kensington);
        } else {
            // Optionally handle the case where the key is not found
            System.out.println("Kensington with ID " + id + " not found.");
        }
    }

    @Override
    public long count(Session session) {
        Query<Long> query = session.createQuery("SELECT COUNT(k) FROM Kensington k", Long.class);
        return query.getSingleResult();
    }

    public List<Kensington> findByLastModifiedAdminId(Session session, Integer adminId) {
        Query<Kensington> query = session.createQuery("Select k FROM Kensington k WHERE k.lastModifiedAdminId = :adminId", Kensington.class);
        query.setParameter("adminId", adminId);
        return query.getResultList();
    }
}
