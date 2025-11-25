using Microsoft.EntityFrameworkCore;
using SistemaDeTarefas.Data;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios.Interfaces;

namespace SistemaDeTarefas.Repositorios
{
    public class TarefaRepositorio : ITarefaRepositorio
    {
        private readonly SistemaTarefasDbContext _context;
        public TarefaRepositorio(SistemaTarefasDbContext sistemaTarefasDbContext)
        {
            _context = sistemaTarefasDbContext;
        }
        public async Task<TarefaModel> BuscarPorId(int id)
        {
            return await _context.Tarefas
                                .Include(x => x.Usuario)
                                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<TarefaModel>> BuscarTodasTarefas()
        {
            return await _context.Tarefas
                                .Include(x => x.Usuario)
                                .ToListAsync();
        }
        public async Task<TarefaModel> Adicionar(TarefaModel tarefa)
        {
            await _context.Tarefas.AddAsync(tarefa);
            await _context.SaveChangesAsync();

            return tarefa;
        }
        public async Task<TarefaModel> Atualizar(TarefaModel tarefa, int id)
        {
            TarefaModel tarefaPorId = await BuscarPorId(id);

            if (tarefaPorId == null)
            {
                throw new Exception($"Usuario {id} nao foi encontrado no banco de dados ");
            }

            tarefaPorId.Nome = tarefa.Nome;
            tarefaPorId.Descricao = tarefa.Descricao;
            tarefaPorId.Status = tarefa.Status;
            tarefaPorId.UsuarioId = tarefa.UsuarioId;

            _context.Tarefas.Update(tarefaPorId);
            await _context.SaveChangesAsync();

            return tarefaPorId;
        }
        public async Task<bool> Apagar(int id)
        {
            TarefaModel tarefaPorId = await BuscarPorId(id);

            if (tarefaPorId == null)
            {
                throw new Exception($"Usuario {id} nao foi encontrado no banco de dados ");
            }

            _context.Tarefas.Remove(tarefaPorId);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
