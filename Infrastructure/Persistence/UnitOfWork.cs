using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Persistence.Contexts;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    // private IUserRepo _userRepo;

    public UnitOfWork(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // public IUserRepo UserRepo => _userRepo ?? new UserRepo(_context);
    public IUserRepo UserRepo => throw new NotImplementedException();
    

    public async Task<bool> SaveChangesAsync() { return (await _context.SaveChangesAsync()) > 0; }

    public async Task BeginAsync() { await _context.BeginTranAsync(); }

    public async Task CommitAsync() { await _context.CommitTranAsync(); }

    public async Task RollbackAsync() { await _context.RollbackTranAsync(); }

    public void Dispose() { _context.Dispose(); }
}