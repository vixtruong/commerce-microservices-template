# Git Commit Message Instructions

When generating Git commit messages, follow the Conventional Commits specification.

## Required format

```text
<type>(<scope>): <short description>
```

For breaking changes, use one or both of these formats:

```text
<type>(<scope>)!: <short description>

BREAKING CHANGE: <description>
```

## Allowed types

- `feat`: add a new feature
- `fix`: fix a bug
- `refactor`: restructure code without changing behavior
- `perf`: improve performance
- `test`: add or update tests
- `docs`: update documentation
- `build`: change the build system or dependencies
- `ci`: change CI/CD configuration
- `chore`: perform maintenance work
- `style`: change formatting only, with no behavior change
- `revert`: revert a previous commit

## Preferred scopes

Use the narrowest relevant scope:

- `gateway`
- `catalog`
- `ordering`
- `domain`
- `application`
- `infrastructure`
- `contracts`
- `api`
- `grpc`
- `graphql`
- `messaging`
- `rabbitmq`
- `outbox`
- `inbox`
- `inventory`
- `media`
- `streaming`
- `persistence`
- `docker`
- `observability`
- `tests`

Other precise scopes such as `readme` or `deps` may be used when they describe the change more accurately.

## Rules

1. Use lowercase for the type, scope, and subject description.
2. Write the subject in the imperative mood.
3. Keep the first line concise, preferably no more than 72 characters.
4. Do not end the subject line with a period.
5. Describe the actual change. Do not use generic subjects such as `update code`, `fix stuff`, or `changes`.
6. Use the narrowest scope that accurately represents the change.
7. Keep one logical change per commit.
8. Do not combine unrelated changes into one commit.
9. Add a commit body when the reason, implementation, or architectural impact is not obvious.
10. In the body, explain why the change was needed and any important implementation decisions.
11. Reference issues in the footer only when an issue number is available:
    - `Closes #123`
    - `Related-to #123`
12. Mark breaking changes with `!` and/or a `BREAKING CHANGE:` footer.
13. Never invent issue numbers, test results, implementation details, or breaking changes.
14. Base the message only on the actual staged changes, diff, or code changes provided.
15. If the staged changes contain unrelated logical changes, recommend splitting them instead of hiding them behind a generic commit message.
16. When asked only for a commit message, output only the proposed commit message without explanation or Markdown fences.

## Scope selection

For changes affecting multiple technical layers, prefer the business capability as the scope.

Use:

```text
feat(catalog): add product video metadata
```

Do not use:

```text
feat(domain): add product video metadata
```

unless the change affects only the domain layer.

## Examples

```text
feat(catalog): add product creation command
feat(streaming): upload product videos using bounded buffers
feat(grpc): add batch product snapshot endpoint
feat(ordering): create order aggregate
feat(messaging): publish order submitted integration event
fix(inventory): prevent duplicate stock reservations
fix(outbox): retry messages after publisher failure
refactor(domain): extract money value object
perf(graphql): batch product queries with data loader
test(ordering): add order aggregate unit tests
docs(readme): document local development setup
build(docker): add rabbitmq and minio containers
chore(deps): update entity framework core packages
```

## Breaking-change example

```text
feat(contracts)!: change catalog product response

Update the product response contract to expose the new media metadata
structure required by catalog consumers.

BREAKING CHANGE: clients must migrate to the new product media fields
```
