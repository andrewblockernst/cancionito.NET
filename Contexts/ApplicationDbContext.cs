using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

//DATABASE CONTEXT FOR THE USER MANAGEMENT. ALSO, EXTENDING THE IDENTITYDBCONTEXT TO MANAGE THE IDENTITY PART
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
    }
}