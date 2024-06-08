using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DecisionTreeNode<T>
{
	Func<Scenario, bool> Decision { get; }
	DecisionTreeNode<T> If { get; set; }
	DecisionTreeNode<T> Else { get; set; }
	T Value { get; set; }


	public DecisionTreeNode(Func<Scenario,bool> decision)
	{
		Decision = decision;
	}

	public DecisionTreeNode(T value)
	{
		Value = value;
	}

	public DecisionTreeNode<T> When(Func<Scenario, bool> decision, Action<DecisionTreeNode<T>> config = null) {
		If = new DecisionTreeNode<T>(decision);
		if(config != null) 
			config(If);
		return this;
	}
	public DecisionTreeNode<T>  WhenNot(Func<Scenario, bool> decision, Action<DecisionTreeNode<T>> config = null)
	{
		Else = new DecisionTreeNode<T>(decision);
		if (config != null)
			config(Else);
		return this;
	}
	public DecisionTreeNode<T> DecideValue(T when, T WhenNot)
	{
		If = new DecisionTreeNode<T>(when);
		Else = new DecisionTreeNode<T>(WhenNot);
		return this;
	}
	public T Decide(Scenario scenario) 
	{
		if (Decision != null)
		{
			var decision = Decision(scenario);

			if (decision && If != null)
				return If.Decide(scenario);

			if (!decision && Else != null)
				return Else.Decide(scenario);
		}
		
		return Value;
	}
}
