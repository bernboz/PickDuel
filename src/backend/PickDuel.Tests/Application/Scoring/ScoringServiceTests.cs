using NSubstitute;
using NUnit.Framework;
using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Application.Scoring;
using PickDuel.Application.Scoring.Interfaces;
using PickDuel.Domain.Entities;
using PickDuel.Domain.Enums;
using PickDuel.Tests.Common;
using PickDuel.Domain.ValueObjects;

namespace PickDuel.Tests.Application.Scoring;

public class ScoringServiceTests
{
    private IScoringCalculator _scoringCalculator;

    private IScoreEventRepository _scoreEventRepository;

    private ScoringService _service;


    [SetUp]
    public void Setup()
    {
        _scoringCalculator = Substitute.For<IScoringCalculator>();

        _scoreEventRepository = Substitute.For<IScoreEventRepository>();

        _service = new ScoringService(
            _scoringCalculator,
            _scoreEventRepository
        );
    }


    [Test]
    public async Task EvaluatePickAsync_ShouldCreateScoreEvents_WhenPickIsValid()
    {
        var user = TestDataFactory.CreateUser();

        var league = TestDataFactory.CreateLeague(user);

        var game = TestDataFactory.CreateGame();

        game.CompleteGame(
            24,
            10
        );

        var pick = TestDataFactory.CreatePick(
            user,
            league,
            game,
            game.HomeTeam,
            1
        );

        pick.Lock();

        var results = new List<ScoringResult>
        {
            new(
                10,
                ScoreEventType.CorrectWinner,
                "Correct winner"
            ),
            new(
                25,
                ScoreEventType.ExactScore,
                "Exact score"
            )
        };

        _scoringCalculator.Calculate(
                pick,
                league.ScoringSettings
            )
            .Returns(results);


        var response = await _service.EvaluatePickAsync(pick);


        Assert.Multiple(() =>
        {
            Assert.That(response.Count, Is.EqualTo(2));
            Assert.That(response.First().Points, Is.EqualTo(10));
            Assert.That(response.Last().Points, Is.EqualTo(25));
            Assert.That(pick.IsScored, Is.True);
        });


        await _scoreEventRepository
            .Received(2)
            .AddAsync(
                Arg.Any<ScoreEvent>(),
                Arg.Any<CancellationToken>()
            );


        await _scoreEventRepository
            .Received(1)
            .SaveChangesAsync(
                Arg.Any<CancellationToken>()
            );
    }


    [Test]
    public void EvaluatePickAsync_ShouldThrow_WhenPickIsNull()
    {
        Assert.ThrowsAsync<ArgumentNullException>(() =>
            _service.EvaluatePickAsync(null!)
        );
    }


    [Test]
    public async Task EvaluatePickAsync_ShouldReturnEmptyResults_WhenCalculatorReturnsNoResults()
    {
        var user = TestDataFactory.CreateUser();

        var league = TestDataFactory.CreateLeague(user);

        var game = TestDataFactory.CreateGame();

        var pick = TestDataFactory.CreatePick(
            user,
            league,
            game,
            game.HomeTeam,
            1
        );

        pick.Lock();

        game.CompleteGame(
            24,
            10
        );


        _scoringCalculator.Calculate(
                pick,
                league.ScoringSettings
            )
            .Returns(Array.Empty<ScoringResult>());


        var response = await _service.EvaluatePickAsync(pick);


        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Empty);
            Assert.That(pick.IsScored, Is.True);
        });


        await _scoreEventRepository
            .DidNotReceive()
            .AddAsync(
                Arg.Any<ScoreEvent>(),
                Arg.Any<CancellationToken>()
            );


        await _scoreEventRepository
            .Received(1)
            .SaveChangesAsync(
                Arg.Any<CancellationToken>()
            );
    }


    [Test]
    public async Task EvaluatePickAsync_ShouldPassCancellationTokenToRepository()
    {
        var tokenSource = new CancellationTokenSource();

        var token = tokenSource.Token;

        var user = TestDataFactory.CreateUser();

        var league = TestDataFactory.CreateLeague(user);

        var game = TestDataFactory.CreateGame();

        game.CompleteGame(
            24,
            10
        );

        var pick = TestDataFactory.CreatePick(
            user,
            league,
            game,
            game.HomeTeam,
            1
        );

        pick.Lock();


        _scoringCalculator.Calculate(
                pick,
                league.ScoringSettings
            )
            .Returns(
                new[]
                {
                    new ScoringResult(
                        10,
                        ScoreEventType.CorrectWinner,
                        "Winner"
                    )
                }
            );


        await _service.EvaluatePickAsync(
            pick,
            token
        );


        await _scoreEventRepository
            .Received()
            .AddAsync(
                Arg.Any<ScoreEvent>(),
                token
            );


        await _scoreEventRepository
            .Received()
            .SaveChangesAsync(
                token
            );
    }


    [Test]
    public void EvaluateGameAsync_ShouldThrow_WhenGameIsNull()
    {
        Assert.ThrowsAsync<ArgumentNullException>(() =>
            _service.EvaluateGameAsync(null!)
        );
    }


    [Test]
    public void EvaluateGameAsync_ShouldThrow_WhenGameIsNotCompleted()
    {
        var game = TestDataFactory.CreateGame();

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.EvaluateGameAsync(game)
        );
    }


    [Test]
    public void Constructor_ShouldThrow_WhenCalculatorIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ScoringService(
                null!,
                _scoreEventRepository
            ));
    }


    [Test]
    public void Constructor_ShouldThrow_WhenRepositoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ScoringService(
                _scoringCalculator,
                null!
            ));
    }
}