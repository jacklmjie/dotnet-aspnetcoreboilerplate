using System.Collections.Generic;
using Xunit;

namespace DesignMode
{
    /// <summary>
    /// 将对象组合成树形结构以表涉"部分-整体"的层次结构
    /// 组合模式使得用户对单个对象和组合对象的使用具有一致性
    /// </summary>
    public class 组合模式
    {
        [Fact]
        public void Test()
        {
            ConcreteCompany root = new ConcreteCompany("总公司");
            root.Add(new HRDepartment("总公司人力资源部"));
            root.Add(new FinanceDepartment("总公司财务部"));

            ConcreteCompany comp = new ConcreteCompany("上海分公司");
            comp.Add(new HRDepartment("上海分公司人力资源部"));
            comp.Add(new FinanceDepartment("上海分公司财务部"));
            root.Add(comp);

            ConcreteCompany comp2 = new ConcreteCompany("杭州办事处");
            comp2.Add(new HRDepartment("杭州办事处人力资源部"));
            comp2.Add(new FinanceDepartment("杭州办事处财务部"));
            root.Add(comp2);

            System.Console.WriteLine("\n结构图");
            root.Display(1);

            System.Console.WriteLine("\n职责");
            root.LineOfDuty();

            System.Console.Read();
        }
        /// <summary>
        /// 公司类 抽象类或接口
        /// </summary>
        abstract class Company
        {
            protected string _name;
            public Company(string name)
            {
                _name = name;
            }
            public abstract void Add(Company c);//增加
            public abstract void Remove(Company c);//移除
            public abstract void Display(int depth);//显示
            public abstract void LineOfDuty();//履行职责
        }
        /// <summary>
        /// 具体公司类 实现接口 树枝节点
        /// </summary>
        class ConcreteCompany : Company
        {
            private List<Company> children = new List<Company>();
            public ConcreteCompany(string name)
                : base(name)
            { }
            public override void Add(Company c)
            {
                children.Add(c);
            }

            public override void Remove(Company c)
            {
                children.Remove(c);
            }

            public override void Display(int depth)
            {
                System.Console.WriteLine(new string('-', depth) + _name);
                foreach (var item in children)
                {
                    item.Display(depth + 2);
                }
            }

            public override void LineOfDuty()
            {
                foreach (var item in children)
                {
                    item.LineOfDuty();
                }
            }
        }

        /// <summary>
        /// 人力资源部 树叶节点
        /// </summary>
        class HRDepartment : Company
        {
            public HRDepartment(string name)
                : base(name)
            { }

            public override void Add(Company c)
            {
            }

            public override void Remove(Company c)
            {
            }

            public override void Display(int depth)
            {
                System.Console.WriteLine(new string('-', depth) + _name);
            }

            public override void LineOfDuty()
            {
                System.Console.WriteLine($"{_name} 员工招聘培训管理");
            }
        }

        /// <summary>
        /// 财务部 树叶节点
        /// </summary>
        class FinanceDepartment : Company
        {
            public FinanceDepartment(string name)
                : base(name)
            { }

            public override void Add(Company c)
            {
            }

            public override void Remove(Company c)
            {
            }

            public override void Display(int depth)
            {
                System.Console.WriteLine(new string('-', depth) + _name);
            }

            public override void LineOfDuty()
            {
                System.Console.WriteLine($"{_name} 公司财务收支管理");
            }
        }
    }
}
