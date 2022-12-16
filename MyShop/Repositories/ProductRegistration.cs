using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop.Context;
using MyShop.Models;
using MyShop.Repositories.Interfaces;
using System.Reflection;

namespace MyShop.Repositories
{
        
    public class ProductRegistration : IProductRegistration
    {
        private readonly DbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public ProductRegistration(DbContext context,IHttpContextAccessor httpContext)
        {
            _context = context;            
            _httpContext = httpContext;
        }

        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            var query = @"Select * from tblProducts";            
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<ProductModel>(query);

                return result;
            }
        }

        public async Task<int> InsertProduct(ProductModel model)
        {

            var querycheck = @"select * from tblProducts where Title=@Title and Catid=@catid";
            int result = 0;
            var tmp = _httpContext.HttpContext.Session.GetString("userid");
            Guid userid=new Guid(tmp);
            model.CreatedBy= userid;
            model.ModifiedBy= userid;
            model.IsDeleted = false;
        
            var query = @"DECLARE @idd UNIQUEIDENTIFIER=NEWID();
                        insert into tblProducts(id,Catid,Title,Images,Price,Descriptions,createdBy,createdDate,
                        modifiedBy,modifiedDate,isDeleted)
                        values(@idd,@Catid,@Title,@Images,@Price,@Desc,@createdBy,GETDATE(),@modifiedBy,GETDATE(),@isDeleted)";
            //int result = 0;
            using (var connection = _context.CreateConnection())
            {
                var resultcheck = await connection.QueryAsync<ProductModel>(querycheck, model);
                if (resultcheck!=null)
                    return -1;
                else
                {
                    result = await connection.ExecuteAsync(query, model);
                }
                

                return result;
            }
        }

        public async Task<int> UpdateProduct(ProductModel model)
        {
            //model.ModifiedBy=?
            model.ModifiedDate= DateTime.Now;
            var findproduct = @"select * from tblProducts where id=@id";            


            var query = @"update tblProducts 
                        set Catid=@Catid,Title=@Title,Images=@Images,Price=@Price,Descriptions=@Descriptions,
                        SpecId=@SpecId,modifiedBy=@modifiedBy,modifiedDate=@modifiedDate,isDeleted=@isDeleted where id=@id";
            
            using (var connection = _context.CreateConnection())
            {
                var resultcheck = await connection.ExecuteAsync(findproduct, model);
                if (resultcheck == 0)
                    return 0;
                var result = await connection.ExecuteAsync(query, model);

                return result;
            }
        }

        public async Task<int> DeleteProduct(DeleteModel model)
        {
            //model.ModifiedBy=?
            model.ModifiedDate = DateTime.Now;
            var findproduct = @"select * from tblProducts where id=@id";           


            var query = @"delete from tblProducts where id=@id";
          
            using (var connection = _context.CreateConnection())
            {
                var resultcheck = await connection.ExecuteAsync(findproduct, model);
                if (resultcheck == 0)
                    return 0;
                var result = await connection.ExecuteAsync(query, model);

                return result;
            }
        }
    }
}
