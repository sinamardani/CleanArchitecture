# AuditTableEntityInterceptor

این اینترسپتور به صورت خودکار فیلدهای Audit و Soft Delete را مدیریت می‌کند.

## قابلیت‌ها

1. **مدیریت فیلدهای Audit:**
   - `CreatedOn` و `CreatedBy` هنگام ایجاد رکورد جدید
   - `LastModifiedOn` و `ModifiedBy` هنگام به‌روزرسانی رکورد
   - جلوگیری از تغییر `CreatedOn` و `CreatedBy` در به‌روزرسانی‌ها

2. **Soft Delete:**
   - تبدیل حذف فیزیکی به حذف منطقی
   - تنظیم خودکار `DeletedOn` و `DeletedBy` هنگام حذف
   - فیلتر خودکار رکوردهای حذف شده از تمامی کوئری‌ها

## قوانین کسب‌وکار

- اگر `DeletedBy == null` و `DeletedOn == null` باشد، رکورد حذف نشده است
- اگر `DeletedBy != null` یا `DeletedOn != null` باشد، رکورد حذف شده است
- تمامی کوئری‌های Select به صورت خودکار رکوردهای حذف شده را فیلتر می‌کنند
- فیلدهای `LastModifiedOn` و `ModifiedBy` nullable هستند و فقط هنگام به‌روزرسانی تنظیم می‌شوند

## نحوه استفاده

### 1. ثبت سرویس‌ها در Program.cs یا Startup.cs:

```csharp
builder.Services.AddPersistence();
```

### 2. استفاده در DbContext:

#### روش 1: استفاده از Interceptor (پیشنهادی برای Audit Fields)

```csharp
services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    var interceptor = sp.GetRequiredService<AuditTableEntityInterceptor>();
    options.UseSqlServer(connectionString)
           .AddInterceptors(interceptor);
});
```

#### روش 2: استفاده از Global Query Filters (پیشنهادی برای Soft Delete Filtering)

برای فیلتر کردن رکوردهای حذف شده، می‌توانید از Global Query Filters استفاده کنید که روش بهینه‌تر و قابل‌اعتمادتری است:

```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // اعمال فیلتر خودکار برای Soft Delete
        modelBuilder.ApplySoftDeleteGlobalQueryFilter();
        
        // سایر کانفیگ‌ها...
    }
}
```

**نکته:** می‌توانید از هر دو روش به صورت همزمان استفاده کنید:
- Interceptor برای مدیریت فیلدهای Audit و Soft Delete در SaveChanges
- Global Query Filters برای فیلتر کردن رکوردهای حذف شده در کوئری‌ها (روش بهینه‌تر)

## نکات مهم

- اینترسپتور فقط برای Entity هایی که `IBaseAuditTableEntity` یا `ISoftDelete` را پیاده‌سازی کرده‌اند کار می‌کند
- برای دسترسی به رکوردهای حذف شده، باید از `IgnoreQueryFilters()` استفاده کنید
- `ICurrentUserService` باید از `HttpContext` شناسه کاربر را دریافت کند

