using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;

namespace FluentValidationLib
{
    public class WidgetsValidator : AbstractValidator<IList<Widget>>
    {
        public WidgetsValidator()
        {
            //RuleFor(inputs => inputs).NotEmpty();
            //RuleFor(inputs => inputs).NotEmpty();

            //RuleFor(inputs => inputs)
            //  //.Must(list => list.Count <= 10)
            //  .Must(list => {
            //      var idGroups = list.GroupBy(i => i.Id).ToList();

            //      var invalidIdGroups = new List<IGrouping<Guid, Widget>>();

            //      foreach (var idGroup in idGroups)
            //      {
            //          var descriptions = idGroup.Select(i => i.Description).ToList();

            //          var uniqueDescriptions = descriptions.Distinct().OrderBy(d => d).ToList();

            //          if (uniqueDescriptions.Count > 1)
            //          {
            //              invalidIdGroups.Add(idGroup);
            //          }
            //      }

            //      return !invalidIdGroups.Any();
            //  });
            ////.WithMessage("No more than 10 orders are allowed");

            //RuleFor(inputs => inputs)
            //  .Must((rootObject, list, context) => {
            //      var idGroups = list.GroupBy(i => i.Id).ToList();

            //      var invalidIdGroups = new List<IGrouping<Guid, Widget>>();

            //      foreach (var idGroup in idGroups)
            //      {
            //          var descriptions = idGroup.Select(i => i.Description).ToList();

            //          var uniqueDescriptions = descriptions.Distinct().OrderBy(d => d).ToList();

            //          if (uniqueDescriptions.Count > 1)
            //          {
            //              invalidIdGroups.Add(idGroup);
            //          }
            //      }

            //      var isValid = !invalidIdGroups.Any();

            //      //if (!isValid)
            //      //{
            //      //    context.M = "Description";
            //      //}

            //      return !invalidIdGroups.Any();
            //  });

            RuleFor(inputs => inputs).Custom((list, context) => {
                var idGroups = list.GroupBy(i => i.Id).ToList();

                var invalidIdGroups = new List<IGrouping<Guid, Widget>>();

                foreach (var idGroup in idGroups)
                {
                    var descriptions = idGroup.Select(i => i.Description).ToList();

                    var uniqueDescriptions = descriptions.Distinct().OrderBy(d => d).ToList();

                    if (uniqueDescriptions.Count > 1)
                    {
                        invalidIdGroups.Add(idGroup);
                    }
                }

                var isValid = !invalidIdGroups.Any();

                if (!isValid)
                {
                    var messageParts = 
                        invalidIdGroups
                        .Select(g => $"Item Id [{g.Key}]:\n{{\n{string.Join(",\n", g.Select(i => $"'{i.Description}'").ToList())}\n}}")
                        .ToList();

                    var message = string.Join("\n", messageParts);

                    context.AddFailure($"The item Groups must contain unique Descriptions:\n{message}");
                }
            });
        }
    }

    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> FullName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                       .MinimumLength(10)
                       .Must(val => val.Split(" ").Length >= 2);
        }

        //public static IRuleBuilderOptions<T, string> AllDescriptionsAreUnique<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    return ruleBuilder
        //        .MinimumLength(10)
        //        .Must(val => val.Split(" ").Length >= 2);
        //}

        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {

            return ruleBuilder.Must((rootObject, list, context) => {
                context.MessageFormatter.AppendArgument("MaxElements", num);
                return list.Count < num;
            })
            .WithMessage("{PropertyName} must contain fewer than {MaxElements} items.");
        }

        public static IRuleBuilderOptions<T, IList<TElement>> Another<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {

            return ruleBuilder.Must((rootObject, list, context) => {
                context.MessageFormatter.AppendArgument("MaxElements", num);
                return list.Count < num;
            })
            .WithMessage("{PropertyName} must contain fewer than {MaxElements} items.");
        }
    }
}
