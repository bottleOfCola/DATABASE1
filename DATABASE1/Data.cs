using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATABASE1
{
    public class Data
    {
        private List<Work> _works = new List<Work>();
        private List<Izdatelstvo> _isdtvos = new List<Izdatelstvo>();
        private List<Human> _humans = new List<Human>();
        private List<Author> _authors = new List<Author>();
        private List<Stepen> _stepens = new List<Stepen>();
        private List<ScienceWork> _scienceWorks = new List<ScienceWork>();

        internal List<Work> Works
            { get { return _works; } }
        internal List<Izdatelstvo> Isdtvos
            { get { return _isdtvos; } }
        internal List<Human> Humans
            { get { return _humans; } }
        internal List <Author> Authors
            { get { return _authors; } }
        internal List<Stepen> Stepens
            { get { return _stepens; } }
        internal List<ScienceWork> ScienceWorks
            { get { return _scienceWorks; } }

        public bool WorkAdd(string txt)
        {
            Work w = new Work(txt);
            foreach(Work work in _works)
            {
                if (work == w)
                    return false;
            }
            _works.Add(w);
            return true;
        }
        public void DeleteWork(string name)
        {
            foreach(Work work in _works)
            {
                if(work.Name == name)
                {
                    _works.Remove(work);
                    return;
                }
            }
        }
        public Work GetWork(string wrk)
        {
            foreach(Work work in _works)
            {
                if(wrk == work.Name)
                    return work;
            }
            return null;
        }
        public bool StepenAdd(string txt)
        {
            Stepen s = new Stepen(txt);
            foreach (Stepen stpn in _stepens)
            {
                if (stpn == s)
                    return false;
            }
            _stepens.Add(s);
            return true;
        }
        public void DeleteStepen(string name)
        {
            foreach (Stepen stpn in _stepens)
            {
                if (stpn.Name == name)
                {
                    _stepens.Remove(stpn);
                    return;
                }
            }
        }
        public Stepen GetStepen(string stpn)
        {
            foreach (Stepen stepen in _stepens)
            {
                if (stpn == stepen.Name)
                    return stepen;
            }
            return null;
        }

        public bool IzdtvoAdd(string txt)
        {
            Izdatelstvo i = new Izdatelstvo(txt);
            foreach(Izdatelstvo iz in _isdtvos)
            {
                if (i == iz)
                    return false;
            }
            _isdtvos.Add(i);
            return true;
        }
        public void DeleteIzdtvo(string name)
        {
            foreach (Izdatelstvo izdatelstvo in _isdtvos)
            {
                if (izdatelstvo.Name == name)
                {
                    _isdtvos.Remove(izdatelstvo);
                    return;
                }
            }
        }
        public Izdatelstvo GetIzdtvo(string name)
        {
            foreach (Izdatelstvo i in _isdtvos)
            {
                if (name == i.Name)
                    return i;
            }
            return null;
        }

        public bool HumanAdd(string fio, DateTime dt, Work work)
        {
            Human hh = new Human(fio,dt,work);
            foreach(Human h in _humans)
            {
                if (h == hh)
                    return false;
            }
            _humans.Add(hh);
            return true;
        }
        public void DeleteHuman(Human human)
        {
            foreach(Human h in _humans)
            {
                if(h== human)
                {
                    _humans.Remove(h);
                    return;
                }
            }
        }
        public Human GetHuman(string fio, string dt, Work work)
        {
            foreach (Human h in _humans)
            {
                if (h.Fio == fio && h.Birthday == dt && h.Work == work)
                {
                    return h;
                }
            }
            return null;
        }

        public bool AuthorAdd(Human human,Stepen stepen, int indx, string psevdonim)
        {
            Author aa = new Author(indx,stepen,human, psevdonim);
            foreach (Author a in _authors)
            {
                if (a == aa)
                    return false;
            }
            _authors.Add(aa);
            return true;
        }
        public void DeleteAuthor(Author author)
        {
            foreach (Author a in _authors)
            {
                if (a == author)
                {
                    _authors.Remove(a);
                    return;
                }
            }
        }
        public Author GetAuthor(string psevdonim)
        {
            foreach (Author a in _authors)
            {
                if (a.Psevdonim == psevdonim)
                {
                    return a;
                }
            }
            return null;
        }
        public bool ScienceWorkAdd(int stranici, string udk, string date, int indx, string workType, Izdatelstvo isdtvo, Author author)
        {
            ScienceWork sw = new ScienceWork(stranici,udk,date,indx,workType,isdtvo,author);
            foreach (ScienceWork s in _scienceWorks)
            {
                if (s == sw)
                    return false;
            }
            _scienceWorks.Add(sw);
            return true;
        }
        public void DeleteScienceWork(ScienceWork sw)
        {
            foreach (ScienceWork s in _scienceWorks)
            {
                if (s == sw)
                {
                    _scienceWorks.Remove(s);
                    return;
                }
            }
        }
        public ScienceWork GetScienceWork(int indx)
        {
            if(indx < _scienceWorks.Count && indx >= 0)
                return _scienceWorks[indx];
            return null;
        }
    }

    public class Work
    {
        private string _name;
        public string Name
        {
            get { return _name; }
        }
        public Work()
        {
            _name = "Нет данных";
        }
        public Work(string name)
        {
            _name = name;
        }
        public static bool operator ==(Work a,Work b)
        {
            return a.Name == b.Name;
        }
        public static bool operator !=(Work a, Work b)
        {
            return a.Name != b.Name;
        }
    }
    public class Human
    {
        private string _fio;
        public string Fio { get { return _fio; } }

        private string _birthday;
        public string Birthday { get { return _birthday; } }

        private Work _work;
        public Work Work { get { return _work; } }

        public Human()
        {
            this._fio = "Нет данных";
            this._work = new Work();
            _birthday = new DateTime(0, 0, 0).GetDateTimeFormats()[0];
        }

        public Human(string fio, DateTime birthday, Work work)
        {
            this._fio = fio;
            this._birthday = birthday.GetDateTimeFormats()[0];
            this._work = work;
        }
        public void Check()
        {
            if (_work is null) _work = new Work();
        }

        public static bool operator !=(Human a, Human b)
        {
            if (a.Fio == b.Fio && a.Birthday == b.Birthday && a.Work == b.Work)
                return false;
            return true;
        }
        public static bool operator ==(Human a, Human b)
        {
            if (a.Fio == b.Fio && a.Birthday == b.Birthday && a.Work == b.Work)
                return true;
            return false;
        }
    }
    public class Stepen
    {
        private string _name;
        public string Name
        {
            get { return _name; }
        }
        public Stepen()
        {
            _name = "Нет данных";
        }
        public Stepen(string name)
        {
            _name = name;
        }
        public static bool operator ==(Stepen a, Stepen b)
        {
            return a.Name == b.Name;
        }
        public static bool operator !=(Stepen a, Stepen b)
        {
            return a.Name != b.Name;
        }
    }

    public class Author
    {
        private string _psevdonim;
        public string Psevdonim { get { return _psevdonim; } }

        private int _indx;
        public int Indx { get { return _indx; } }

        private Stepen _stepen;
        public Stepen Stepen { get { return _stepen; } }

        private Human _human;
        public Human Human { get { return _human; } }

        public Author()
        {
            this._indx = 0;
            this._stepen = new Stepen();
            this._human = new Human();
            this._psevdonim = "Нет данных";
        }
        public Author(int indx, Stepen stepen, Human human, string psevdonim)
        {
            this._indx = indx;
            this._stepen = stepen;
            this._human = human;
            this._psevdonim = psevdonim;
        }

        public void Check()
        {
            if (this._human is null)
            {
                _human = new Human();
            }
            if(this._stepen is null) _stepen = new Stepen();
        }
        public static bool operator ==(Author a, Author b)
        {
            return a.Psevdonim == b.Psevdonim;
        }
        public static bool operator !=(Author a, Author b)
        {
            return a.Psevdonim != b.Psevdonim;
        }
    }

    public class Izdatelstvo
    {
        private string _name;
        public string Name { get { return _name; } }

        public Izdatelstvo()
        {
            this._name = "Нет данных";
        }

        public Izdatelstvo(string name)
        {
            this._name = name;
        }

        public static bool operator ==(Izdatelstvo a, Izdatelstvo b)
        {
            return a.Name == b.Name;
        }
        public static bool operator !=(Izdatelstvo a, Izdatelstvo b)
        {
            return a.Name != b.Name;
        }
    }

    public class ScienceWork
    {
        private int _stranici;
        public int Stranici { get { return _stranici; } }

        private string _udk;
        public string Udk { get { return _udk; } }

        private string _date;
        public string Date { get { return _date; } }

        private int _indx;
        public int Indx { get { return _indx; } }

        private string _workType;
        public string WorkType { get { return _workType; } }

        private Izdatelstvo _isdtvo;
        public Izdatelstvo Isdtvo { get { return _isdtvo; } }

        private Author _author;
        public Author Author { get { return _author; } }

        public ScienceWork(int stranici, string udk, string date, int indx, string workType, Izdatelstvo isdtvo, Author author)
        {
            _stranici = stranici;
            _udk = udk;
            _date = date;
            _indx = indx;
            _workType = workType;
            _isdtvo = isdtvo;
            _author = author;
        }

        public void Check()
        {
            if (_isdtvo is null) _isdtvo = new Izdatelstvo();
            if (_author is null) _author = new Author();
        }
        public static bool operator ==(ScienceWork a, ScienceWork b)
        {
            if(a.Udk == b.Udk && a.Isdtvo == b.Isdtvo && a.Author == b.Author && a.Stranici == b.Stranici && a.Date == b.Date && a.WorkType == b.WorkType)
                return true;
            return false;
        }
        public static bool operator !=(ScienceWork a, ScienceWork b)
        {
            if (a.Udk == b.Udk && a.Isdtvo == b.Isdtvo && a.Author == b.Author && a.Stranici == b.Stranici && a.Date == b.Date && a.WorkType == b.WorkType)
                return false;
            return true;
        }
    }
}
