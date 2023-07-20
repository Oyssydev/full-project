using ConverterAPI.Data;
using ConverterClassLib;
using Microsoft.EntityFrameworkCore;

namespace ConverterAPI.Services
{
    public interface IEtudiantService
    {
        Task<List<Etudiant>> GetEtdAsync();
        Task<bool> InsertAsync(Etudiant etd);
        Task<Etudiant> GetEtudiantByIdAsync(int id);
        Task<bool> PutEtudiantAsync(Etudiant etd);
        Task<bool> DeleteAsync(int id);
    }
    public class EtudiantService : IEtudiantService
    {
        public readonly DataContext _context;
        public EtudiantService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Etudiant>> GetEtdAsync()
        {
            return await _context.Etudiants.ToListAsync();
        }
    
        public async Task<bool> InsertAsync(Etudiant etd)
        {
            try
            {
                _context.Etudiants.Add(etd);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Etudiant> GetEtudiantByIdAsync(int id)
        {
            try
            {
                var task = await _context.Etudiants.FindAsync(id);
                await _context.SaveChangesAsync();
                return task;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> PutEtudiantAsync(Etudiant etd)
        {
            try
            {
                var dbEtd = await _context.Etudiants.FindAsync(etd.id);
                dbEtd.firstName = etd.firstName;
                dbEtd.LastName = etd.LastName;
                dbEtd.Deparetement = etd.Deparetement;
                dbEtd.Class = etd.Class;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var etd = await _context.Etudiants.FindAsync(id);
                _context.Etudiants.Remove(etd);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
